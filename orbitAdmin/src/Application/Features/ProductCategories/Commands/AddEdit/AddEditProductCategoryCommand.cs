using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.ProductCategories.Commands.AddEdit
{
    public class AddEditProductCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameGe { get; set; }
        public string DescriptionAr1 { get; set; }
        public string DescriptionAr2 { get; set; }
        public string DescriptionAr3 { get; set; }
        public string DescriptionAr4 { get; set; }


        public string DescriptionEn1 { get; set; }
        public string DescriptionEn2 { get; set; }
        public string DescriptionEn3 { get; set; }
        public string DescriptionEn4 { get; set; }


        public string DescriptionGe1 { get; set; }
        public string DescriptionGe2 { get; set; }
        public string DescriptionGe3 { get; set; }
        public string DescriptionGe4 { get; set; }

        public int? ParentCategoryId { get; set; }

        public int Order { get; set; } = 0;
        public string ImageDataURL1 { get; set; }
        public string ImageDataURL2 { get; set; }
        public string ImageDataURL3 { get; set; }

        public UploadRequest UploadRequestURL1 { get; set; }
        public UploadRequest UploadRequestURL2{ get; set; }
        public UploadRequest UploadRequestURL3{ get; set; }
    }

    internal class AddEditProductCategoryCommandHandler : IRequestHandler<AddEditProductCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCategoryCommandHandler> _localizer;

        public AddEditProductCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductCategoryCommand command, CancellationToken cancellationToken)
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
                var productCategory = _mapper.Map<ProductCategory>(command);

                productCategory.ParentCategoryId = productCategory.ParentCategoryId == 0 ? null : productCategory.ParentCategoryId;
              
                
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

                await _unitOfWork.Repository<ProductCategory>().AddAsync(productCategory);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                return await Result<int>.SuccessAsync(productCategory.Id, _localizer["Product Category Saved"]);
            }
            else
            {
                var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
                if (productCategory != null)
                {
                    productCategory.NameEn = command.NameEn ?? productCategory.NameEn;
                    productCategory.NameAr = command.NameAr ?? productCategory.NameAr;
                    productCategory.NameGe = command.NameGe ?? productCategory.NameGe;
                    productCategory.DescriptionEn1 = command.DescriptionEn1 ?? productCategory.DescriptionEn1;
                    productCategory.DescriptionEn2 = command.DescriptionEn2 ?? productCategory.DescriptionEn2;
                    productCategory.DescriptionEn3 = command.DescriptionEn3 ?? productCategory.DescriptionEn3;
                    productCategory.DescriptionEn4 = command.DescriptionEn4 ?? productCategory.DescriptionEn4;
                    productCategory.DescriptionAr1 = command.DescriptionAr1 ?? productCategory.DescriptionAr1;
                    productCategory.DescriptionAr2 = command.DescriptionAr2 ?? productCategory.DescriptionAr2;
                    productCategory.DescriptionAr3 = command.DescriptionAr3 ?? productCategory.DescriptionAr3;
                    productCategory.DescriptionAr4 = command.DescriptionAr4 ?? productCategory.DescriptionAr4;       
                    
                    productCategory.DescriptionGe1 = command.DescriptionGe1 ?? productCategory.DescriptionGe1;
                    productCategory.DescriptionGe2 = command.DescriptionGe2 ?? productCategory.DescriptionGe2;
                    productCategory.DescriptionGe3 = command.DescriptionGe3 ?? productCategory.DescriptionGe3;
                    productCategory.DescriptionGe4 = command.DescriptionGe4 ?? productCategory.DescriptionGe4;

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


                    productCategory.ParentCategoryId = (command.ParentCategoryId == 0) ? productCategory.ParentCategoryId : command.ParentCategoryId;
                    productCategory.Order = (command.Order == 0) ? productCategory.Order : command.Order;
                    await _unitOfWork.Repository<ProductCategory>().UpdateAsync(productCategory);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(productCategory.Id, _localizer["ProductCategory Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ProductCategory Not Found!"]);
                }
            }
        }
    }
}

