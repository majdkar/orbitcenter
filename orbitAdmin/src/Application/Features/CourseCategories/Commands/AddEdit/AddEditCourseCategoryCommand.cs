using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.CourseCategories.Commands.AddEdit
{
    public class AddEditCourseCategoryCommand : IRequest<Result<int>>
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

    internal class AddEditCourseCategoryCommandHandler : IRequestHandler<AddEditCourseCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditCourseCategoryCommandHandler> _localizer;

        public AddEditCourseCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditCourseCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCourseCategoryCommand command, CancellationToken cancellationToken)
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
                var CourseCategory = _mapper.Map<CourseCategory>(command);

                CourseCategory.ParentCategoryId = CourseCategory.ParentCategoryId == 0 ? null : CourseCategory.ParentCategoryId;



                if (uploadRequestUrl1 != null)
                {
                    CourseCategory.ImageDataURL1 = _uploadService.UploadAsync(uploadRequestUrl1);
                }
                if (uploadRequestUrl2 != null)
                {
                    CourseCategory.ImageDataURL2 = _uploadService.UploadAsync(uploadRequestUrl2);
                }
                if (uploadRequestUrl3 != null)
                {
                    CourseCategory.ImageDataURL3 = _uploadService.UploadAsync(uploadRequestUrl3);
                }

                await _unitOfWork.Repository<CourseCategory>().AddAsync(CourseCategory);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseCategoriesCacheKey);
                return await Result<int>.SuccessAsync(CourseCategory.Id, _localizer["Course Category Saved"]);
            }
            else
            {
                var CourseCategory = await _unitOfWork.Repository<CourseCategory>().GetByIdAsync(command.Id);
                if (CourseCategory != null)
                {
                    CourseCategory.NameEn = command.NameEn ?? CourseCategory.NameEn;
                    CourseCategory.NameAr = command.NameAr ?? CourseCategory.NameAr;
                    CourseCategory.NameGe = command.NameGe ?? CourseCategory.NameGe;
                    CourseCategory.DescriptionEn1 = command.DescriptionEn1 ?? CourseCategory.DescriptionEn1;
                    CourseCategory.DescriptionEn2 = command.DescriptionEn2 ?? CourseCategory.DescriptionEn2;
                    CourseCategory.DescriptionEn3 = command.DescriptionEn3 ?? CourseCategory.DescriptionEn3;
                    CourseCategory.DescriptionEn4 = command.DescriptionEn4 ?? CourseCategory.DescriptionEn4;
                    CourseCategory.DescriptionAr1 = command.DescriptionAr1 ?? CourseCategory.DescriptionAr1;
                    CourseCategory.DescriptionAr2 = command.DescriptionAr2 ?? CourseCategory.DescriptionAr2;
                    CourseCategory.DescriptionAr3 = command.DescriptionAr3 ?? CourseCategory.DescriptionAr3;
                    CourseCategory.DescriptionAr4 = command.DescriptionAr4 ?? CourseCategory.DescriptionAr4;       
                    
                    CourseCategory.DescriptionGe1 = command.DescriptionGe1 ?? CourseCategory.DescriptionGe1;
                    CourseCategory.DescriptionGe2 = command.DescriptionGe2 ?? CourseCategory.DescriptionGe2;
                    CourseCategory.DescriptionGe3 = command.DescriptionGe3 ?? CourseCategory.DescriptionGe3;
                    CourseCategory.DescriptionGe4 = command.DescriptionGe4 ?? CourseCategory.DescriptionGe4;

                    if (uploadRequestUrl1 != null)
                    {
                        CourseCategory.ImageDataURL1 = _uploadService.UploadAsync(uploadRequestUrl1);
                    }
                    if (uploadRequestUrl2 != null)
                    {
                        CourseCategory.ImageDataURL2 = _uploadService.UploadAsync(uploadRequestUrl2);
                    }
                    if (uploadRequestUrl3 != null)
                    {
                        CourseCategory.ImageDataURL3 = _uploadService.UploadAsync(uploadRequestUrl3);
                    }

                    CourseCategory.ParentCategoryId = (command.ParentCategoryId == 0) ? CourseCategory.ParentCategoryId : command.ParentCategoryId;
                    CourseCategory.Order = (command.Order == 0) ? CourseCategory.Order : command.Order;
                    await _unitOfWork.Repository<CourseCategory>().UpdateAsync(CourseCategory);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseCategoriesCacheKey);
                    return await Result<int>.SuccessAsync(CourseCategory.Id, _localizer["CourseCategory Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["CourseCategory Not Found!"]);
                }
            }
        }
    }
}

