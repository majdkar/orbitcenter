using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Commands.AddEdit
{
    public class AddEditCourseOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

internal class AddEditCourseOfferCommandHandler : IRequestHandler<AddEditCourseOfferCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditCourseOfferCommandHandler> _localizer;
    private readonly IUnitOfWork<int> _unitOfWork;

    public AddEditCourseOfferCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCourseOfferCommandHandler> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Result<int>> Handle(AddEditCourseOfferCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == 0)
        {

                var CourseOffer = _mapper.Map<CourseOffer>(command);

           
            await _unitOfWork.Repository<CourseOffer>().AddAsync(CourseOffer);
                try
                {
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseOffersCacheKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return await Result<int>.SuccessAsync(CourseOffer.Id, _localizer["Course Offer Saved"]);
            
        }
        else
        {
            var CourseOffer = await _unitOfWork.Repository<CourseOffer>().GetByIdAsync(command.Id);
            if (CourseOffer != null)
            {
                CourseOffer.NewPrice = command.NewPrice ?? CourseOffer.NewPrice;
                CourseOffer.DiscountRatio = command.DiscountRatio ?? CourseOffer.DiscountRatio;
                CourseOffer.StartDate = command.StartDate ?? CourseOffer.StartDate;
                CourseOffer.EndDate = command.EndDate ?? CourseOffer.EndDate;
                await _unitOfWork.Repository<CourseOffer>().UpdateAsync(CourseOffer);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseOffersCacheKey);
                return await Result<int>.SuccessAsync(CourseOffer.Id, _localizer["Course Offer Updated"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Course Offer Not Found!"]);
            }
        }
    }
}
