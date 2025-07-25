﻿using AutoMapper;
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


namespace SchoolV01.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditCompanyProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameGe { get; set; }


        public string DescriptionEn1 { get; set; }
        public string DescriptionEn2 { get; set; }
        public string DescriptionEn3 { get; set; }
        public string DescriptionEn4 { get; set; }

        public string DescriptionAr1 { get; set; }
        public string DescriptionAr2 { get; set; }
        public string DescriptionAr3 { get; set; }
        public string DescriptionAr4 { get; set; }
        public string DescriptionGe1 { get; set; }
        public string DescriptionGe2 { get; set; }
        public string DescriptionGe3 { get; set; }
        public string DescriptionGe4 { get; set; }

        public string Code { get; set; }

        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }

        public int? ProductParentCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSubSubCategoryId { get; set; }
        public int? ProductSubSubSubCategoryId { get; set; }

        public int? ProductDefaultCategoryId { get; set; }



        public decimal? Price { get; set; }
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public bool IsRecent { get; set; } = false;

        public string Plan { get; set; }

        public string ProductImageUrl1 { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }

        public UploadRequest UploadRequestURL1 { get; set; }
        public UploadRequest UploadRequestURL2 { get; set; }
        public UploadRequest UploadRequestURL3 { get; set; }

        public string Keywords { get; set; }
        public string SeoDescription { get; set; }

    }

    internal class AddEditProductCommandHandler : IRequestHandler<AddEditCompanyProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCompanyProductCommand command, CancellationToken cancellationToken)
        {



            var uploadRequestUrl1 = command.UploadRequestURL1;
            var uploadRequestUrl2 = command.UploadRequestURL2;
            var uploadRequestUrl3 = command.UploadRequestURL3;


            if (uploadRequestUrl1 != null)
            {
                uploadRequestUrl1.FileName = $"{Path.GetRandomFileName()}{uploadRequestUrl1.Extension}";
            }
            if (uploadRequestUrl2 != null)
            {
                uploadRequestUrl2.FileName = $"{Path.GetRandomFileName()}{uploadRequestUrl2.Extension}";
            }
            if (uploadRequestUrl3 != null)
            {
                uploadRequestUrl3.FileName = $"{Path.GetRandomFileName()}{uploadRequestUrl3.Extension}";
            }


            if (command.Id == 0)
            {

                var product = _mapper.Map<Product>(command);



                if (uploadRequestUrl1 != null)
                {
                    product.ProductImageUrl1 = _uploadService.UploadAsync(uploadRequestUrl1);
                }
                if (uploadRequestUrl2 != null)
                {
                    product.ProductImageUrl2 = _uploadService.UploadAsync(uploadRequestUrl2);
                }
                if (uploadRequestUrl3 != null)
                {
                    product.ProductImageUrl3 = _uploadService.UploadAsync(uploadRequestUrl3);
                }
                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductsCacheKey);


                // Call webhook
                //using (HttpClient client = new HttpClient())
                //{
                //    var jsonPayload = $"{{\"id\":{product.Id},\"name\":\"{product.NameAr}\",\"nameEn\":\"{product.NameEn}\"}}";

                //    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                //    var response = await client.PostAsync("https://majood.app.n8n.cloud/webhook-test/7168809f-92d1-4c06-a661-27d62333a82e", content);

                //    if (response.IsSuccessStatusCode)
                //        return await Result<int>.SuccessAsync(product.Id, _localizer["Product Saved"]);
                //    else
                //        return await Result<int>.FailAsync(_localizer["Product Not Saved"]);
                //}
                      return await Result<int>.SuccessAsync(product.Id, _localizer["Product Saved"]);


            }
            else
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
                if (product != null)
                {
                    product.NameAr = command.NameAr ?? product.NameAr;
                    product.NameEn = command.NameEn ?? product.NameEn;

                    product.DescriptionAr1 = command.DescriptionAr1;
                    product.DescriptionAr2 = command.DescriptionAr2;
                    product.DescriptionAr3 = command.DescriptionAr3;
                    product.DescriptionAr4 = command.DescriptionAr4;

                    product.DescriptionEn1 = command.DescriptionEn1;
                    product.DescriptionEn2 = command.DescriptionEn2;
                    product.DescriptionEn3 = command.DescriptionEn3;
                    product.DescriptionEn4 = command.DescriptionEn4;


                    product.DescriptionGe1 = command.DescriptionGe1;
                    product.DescriptionGe2 = command.DescriptionGe2;
                    product.DescriptionGe3 = command.DescriptionGe3;
                    product.DescriptionGe4 = command.DescriptionGe4;
                    product.SeoDescription = command.SeoDescription;
                    product.Keywords = command.Keywords;

                    if (uploadRequestUrl1 != null)
                    {
                        product.ProductImageUrl1 = _uploadService.UploadAsync(uploadRequestUrl1);
                    }
                    if (uploadRequestUrl2 != null)
                    {
                        product.ProductImageUrl2 = _uploadService.UploadAsync(uploadRequestUrl2);
                    }
                    if (uploadRequestUrl3 != null)
                    {
                        product.ProductImageUrl3 = _uploadService.UploadAsync(uploadRequestUrl3);
                    }

                    product.Code = command.Code;
                    product.EndpointAr = command.EndpointAr;
                    product.EndpointEn = command.EndpointEn;
                    product.EndpointGe = command.EndpointGe;

                    product.ProductSubSubCategoryId = command.ProductSubSubCategoryId == 0 ? null : command.ProductSubSubCategoryId;
                    product.ProductSubSubSubCategoryId = command.ProductSubSubSubCategoryId == 0 ? null : command.ProductSubSubSubCategoryId;

                    //product.SizeId = command.SizeId == 0 ? null : command.SizeId;
                    product.ProductParentCategoryId = command.ProductParentCategoryId;
                    product.ProductSubCategoryId = command.ProductSubCategoryId;
                    product.ProductDefaultCategoryId = command.ProductDefaultCategoryId;
       

                    product.Price = !command.Price.HasValue ? product.Price : command.Price;
                    product.Order = command.Order;
                    product.IsVisible = command.IsVisible;
                    product.IsRecent = command.IsRecent;
                    product.Plan = command.Plan;
               
                  
                    await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductsCacheKey);
                    return await Result<int>.SuccessAsync(product.Id, _localizer["Product Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
                }
            }
        }
    }
}