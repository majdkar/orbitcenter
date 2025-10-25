using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Commands.AddEdit
{
    public partial class AddEditCourseOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int CourseId { get; set; }

        public string Status { get; set; }
        public string ClientType { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderNumber { get; set; }

        public decimal Price { get; set; }
        public int? PayTypeId { get; set; }

        public string PaymentTransactionNumber { get; set; }
    }

    internal class AddEditCourseOrderCommandHandler : IRequestHandler<AddEditCourseOrderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditCourseCommandHandler> _localizer;

        public AddEditCourseOrderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditCourseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCourseOrderCommand command, CancellationToken cancellationToken)
        {




            if (command.Id == 0)
            {

                var courseOrder = _mapper.Map<CourseOrder>(command);
         

                var lastOrder = await _unitOfWork.Repository<CourseOrder>()
                    .Entities
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;

                if (lastOrder != null && !string.IsNullOrEmpty(lastOrder.OrderNumber))
                {
                    var numericPart = lastOrder.OrderNumber.Substring(1);
                    if (int.TryParse(numericPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }
                courseOrder.OrderNumber = $"C{nextNumber:D4}";

                await _unitOfWork.Repository<CourseOrder>().AddAsync(courseOrder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseOrdersCacheKey);
                return await Result<int>.SuccessAsync(courseOrder.Id, _localizer["Course Order Saved"]);
            }
            else
            {
                var Course = await _unitOfWork.Repository<CourseOrder>().GetByIdAsync(command.Id);
                if (Course != null)
                {
                    Course.OrderNumber = command.OrderNumber ?? Course.OrderNumber;
                    Course.Status = command.Status ?? Course.Status;
                    Course.PaymentStatus = command.PaymentStatus ?? Course.PaymentStatus;
                    Course.ClientId = command.ClientId;
                    Course.CourseId = command.CourseId;
                    Course.OrderDate = command.OrderDate;
                    Course.Notes = command.Notes ?? Course.Notes;
                    Course.Price = command.Price;
                    Course.PayTypeId = command.PayTypeId;
                    Course.PaymentTransactionNumber = command.PaymentTransactionNumber;

                    await _unitOfWork.Repository<CourseOrder>().UpdateAsync(Course);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseOrdersCacheKey);
                    return await Result<int>.SuccessAsync(Course.Id, _localizer["Course Order Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Course Not Found!"]);
                }
            }
        }
    }
}