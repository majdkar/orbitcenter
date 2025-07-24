using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;

namespace SchoolV01.Application.Features.Courses.Commands.AddEdit
{
    public partial class AddEditCompanyCourseCommand : IRequest<Result<int>>
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
        public int? CourseParentCategoryId { get; set; }
        public int? CourseSubCategoryId { get; set; }
        public int? CourseSubSubCategoryId { get; set; }
        public int? CourseSubSubSubCategoryId { get; set; }

        public int? CourseDefaultCategoryId { get; set; }



        public decimal? Price { get; set; }
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public bool IsRecent { get; set; } = false;

        public string Plan { get; set; }

        public string CourseImageUrl1 { get; set; }
        public string CourseImageUrl2 { get; set; }
        public string CourseImageUrl3 { get; set; }

        public UploadRequest UploadRequestURL1 { get; set; }
        public UploadRequest UploadRequestURL2 { get; set; }
        public UploadRequest UploadRequestURL3 { get; set; }

        public string Keywords { get; set; }
        public string SeoDescription { get; set; }
        public string TeacherNameAr { get; set; }
        public string TeacherNameEn { get; set; }
        public string TeacherNameGe { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? StartEnd { get; set; }

        public int NumSesstions { get; set; }
        public int NumMaxStudent { get; set; }

        public int? CourseTypeId { get; set; }
    }

    internal class AddEditCourseCommandHandler : IRequestHandler<AddEditCompanyCourseCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditCourseCommandHandler> _localizer;

        public AddEditCourseCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditCourseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCompanyCourseCommand command, CancellationToken cancellationToken)
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

                var Course = _mapper.Map<Course>(command);



                if (uploadRequestUrl1 != null)
                {
                    Course.CourseImageUrl1 = _uploadService.UploadAsync(uploadRequestUrl1);
                }
                if (uploadRequestUrl2 != null)
                {
                    Course.CourseImageUrl2 = _uploadService.UploadAsync(uploadRequestUrl2);
                }
                if (uploadRequestUrl3 != null)
                {
                    Course.CourseImageUrl3 = _uploadService.UploadAsync(uploadRequestUrl3);
                }
                await _unitOfWork.Repository<Course>().AddAsync(Course);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCoursesCacheKey);
                return await Result<int>.SuccessAsync(Course.Id, _localizer["Course Saved"]);
            }
            else
            {
                var Course = await _unitOfWork.Repository<Course>().GetByIdAsync(command.Id);
                if (Course != null)
                {
                    Course.NameAr = command.NameAr ?? Course.NameAr;
                    Course.NameEn = command.NameEn ?? Course.NameEn;
                    Course.NameGe = command.NameGe ?? Course.NameGe;

                    Course.DescriptionAr1 = command.DescriptionAr1;
                    Course.DescriptionAr2 = command.DescriptionAr2;
                    Course.DescriptionAr3 = command.DescriptionAr3;
                    Course.DescriptionAr4 = command.DescriptionAr4;

                    Course.DescriptionEn1 = command.DescriptionEn1;
                    Course.DescriptionEn2 = command.DescriptionEn2;
                    Course.DescriptionEn3 = command.DescriptionEn3;
                    Course.DescriptionEn4 = command.DescriptionEn4;


                    Course.DescriptionGe1 = command.DescriptionGe1;
                    Course.DescriptionGe2 = command.DescriptionGe2;
                    Course.DescriptionGe3 = command.DescriptionGe3;
                    Course.DescriptionGe4 = command.DescriptionGe4;
                    Course.SeoDescription = command.SeoDescription;
                    Course.Keywords = command.Keywords;

                    if (uploadRequestUrl1 != null)
                    {
                        Course.CourseImageUrl1 = _uploadService.UploadAsync(uploadRequestUrl1);
                    }
                    if (uploadRequestUrl2 != null)
                    {
                        Course.CourseImageUrl2 = _uploadService.UploadAsync(uploadRequestUrl2);
                    }
                    if (uploadRequestUrl3 != null)
                    {
                        Course.CourseImageUrl3 = _uploadService.UploadAsync(uploadRequestUrl3);
                    }

                    Course.Code = command.Code;

                    Course.CourseSubSubCategoryId = command.CourseSubSubCategoryId == 0 ? null : command.CourseSubSubCategoryId;
                    Course.CourseSubSubSubCategoryId = command.CourseSubSubSubCategoryId == 0 ? null : command.CourseSubSubSubCategoryId;

                    //Course.SizeId = command.SizeId == 0 ? null : command.SizeId;
                    Course.CourseParentCategoryId = command.CourseParentCategoryId;
                    Course.CourseSubCategoryId = command.CourseSubCategoryId;
                    Course.CourseDefaultCategoryId = command.CourseDefaultCategoryId;
       

                    Course.Price = !command.Price.HasValue ? Course.Price : command.Price;
                    Course.Order = command.Order;
                    Course.IsVisible = command.IsVisible;
                    Course.IsRecent = command.IsRecent;
                    Course.Plan = command.Plan;
                    Course.TeacherNameAr = command.TeacherNameAr;
                    Course.TeacherNameEn = command.TeacherNameEn;
                    Course.TeacherNameGe = command.TeacherNameGe;
                    Course.StartDate = command.StartDate;
                    Course.StartEnd = command.StartEnd;
                    Course.NumSesstions = command.NumSesstions;
                    Course.NumMaxStudent = command.NumMaxStudent;
                    Course.CourseTypeId = command.CourseTypeId;
                    Course.EndpointAr = command.EndpointAr;
                    Course.EndpointEn = command.EndpointEn;
                    Course.EndpointGe = command.EndpointGe;
               
                  
                    await _unitOfWork.Repository<Course>().UpdateAsync(Course);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCoursesCacheKey);
                    return await Result<int>.SuccessAsync(Course.Id, _localizer["Course Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Course Not Found!"]);
                }
            }
        }
    }
}