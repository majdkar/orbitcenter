using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Orders;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SchoolV01.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditProductOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }


        public string Status { get; set; }
        public string ClientType { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderNumber { get; set; }

        public decimal TotalPrice { get; set; }

        // كل العناصر (المنتجات) داخل الطلب
        public ICollection<ProductOrderItem> Items { get; set; } = new List<ProductOrderItem>();
    }

    internal class AddEditProductOrderCommandHandler : IRequestHandler<AddEditProductOrderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductOrderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductOrderCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                // 🆕 إنشاء طلب جديد
                var productOrder = _mapper.Map<ProductOrder>(command);

                // توليد رقم الطلب
                var lastOrder = await _unitOfWork.Repository<ProductOrder>()
                    .Entities
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                int nextNumber = 1;
                if (lastOrder != null && !string.IsNullOrEmpty(lastOrder.OrderNumber))
                {
                    var numericPart = lastOrder.OrderNumber.Substring(1);
                    if (int.TryParse(numericPart, out int lastNumber))
                        nextNumber = lastNumber + 1;
                }
                productOrder.OrderNumber = $"C{nextNumber:D4}";

                // إضافة العناصر بدون تكرار
                foreach (var item in command.Items)
                {
                    if (!productOrder.Items.Any(i => i.ProductId == item.ProductId))
                    {
                        productOrder.Items.Add(new ProductOrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        });
                    }
                }

                // حساب المجموع الكلي
                productOrder.TotalPrice = productOrder.Items.Sum(i => i.UnitPrice * i.Quantity);

                await _unitOfWork.Repository<ProductOrder>().AddAsync(productOrder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductOrdersCacheKey);

                return await Result<int>.SuccessAsync(productOrder.Id, _localizer["Product Order Saved"]);
            }
            else
            {
                // ✏️ تعديل طلب موجود
                var productOrder = await _unitOfWork.Repository<ProductOrder>()
                    .Entities
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (productOrder == null)
                    return await Result<int>.FailAsync(_localizer["Product Order Not Found!"]);

                // تحديث الخصائص الأساسية
                productOrder.Status = command.Status ?? productOrder.Status;
                productOrder.PaymentStatus = command.PaymentStatus ?? productOrder.PaymentStatus;
                productOrder.ClientId = command.ClientId;
                productOrder.OrderDate = command.OrderDate;
                productOrder.Notes = command.Notes ?? productOrder.Notes;

                // تحديث العناصر بدون تكرار
                foreach (var item in command.Items)
                {
                    var existingItem = productOrder.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (existingItem != null)
                    {
                        // تحديث الكمية والسعر
                        existingItem.Quantity = item.Quantity;
                        existingItem.UnitPrice = item.UnitPrice;
                    }
                    else
                    {
                        // إضافة عنصر جديد
                        productOrder.Items.Add(new ProductOrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        });
                    }
                }

                // إعادة حساب المجموع الكلي
                productOrder.TotalPrice = productOrder.Items.Sum(i => i.UnitPrice * i.Quantity);

                await _unitOfWork.Repository<ProductOrder>().UpdateAsync(productOrder);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductOrdersCacheKey);

                return await Result<int>.SuccessAsync(productOrder.Id, _localizer["Product Order Updated"]);
            }
        }
    }
}