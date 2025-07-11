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
    public class AddEditCourseSeoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public int CourseId { get; set; }


        public string MetaTitleAr { get; set; }
        public string MetaTitleEn { get; set; }
        public string MetaTitleGe { get; set; }


        public string MetaNameAr { get; set; }
        public string MetaNameEn { get; set; }
        public string MetaNameGe { get; set; }

        public string MetaUrlAr { get; set; }
        public string MetaUrlEn { get; set; }
        public string MetaUrlGe { get; set; }


        public string MetaKeywordsAr { get; set; }
        public string MetaKeywordsEn { get; set; }
        public string MetaKeywordsGe { get; set; }

        public string MetaDescriptionsAr { get; set; }
        public string MetaDescriptionsEn { get; set; }
        public string MetaDescriptionsGe { get; set; }


        public string ImageAlt1Ar { get; set; }
        public string ImageAlt1En { get; set; }
        public string ImageAlt1Ge { get; set; }

        public string ImageAlt2Ar { get; set; }
        public string ImageAlt2En { get; set; }
        public string ImageAlt2Ge { get; set; }

        public string ImageAlt3Ar { get; set; }
        public string ImageAlt3En { get; set; }
        public string ImageAlt3Ge { get; set; }

        public string ImageAlt4Ar { get; set; }
        public string ImageAlt4En { get; set; }
        public string ImageAlt4Ge { get; set; }


        public string MetaRobots { get; set; }

    }
}

internal class AddEditCourseSeoCommandHandler : IRequestHandler<AddEditCourseSeoCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditCourseSeoCommandHandler> _localizer;
    private readonly IUnitOfWork<int> _unitOfWork;

    public AddEditCourseSeoCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCourseSeoCommandHandler> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Result<int>> Handle(AddEditCourseSeoCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == 0)
        {

            var CourseSeo = _mapper.Map<CourseSeo>(command);



            if (command.MetaDescriptionsAr != null)
            {
                if (command.MetaDescriptionsAr.Length > 320)
                {
                    return await Result<int>.FailAsync(_localizer["Descriptions in Arabic 320 characters maximum allowed!"]);

                }
            }


            if (command.MetaDescriptionsEn != null)
            {
                if (command.MetaDescriptionsEn.Length > 160)
                {
                    return await Result<int>.FailAsync(_localizer["Descriptions in English 160 characters maximum allowed!"]);

                }
            }

            if (command.MetaDescriptionsGe != null)
            {
                if (command.MetaDescriptionsGe.Length > 160)
                {
                    return await Result<int>.FailAsync(_localizer["Descriptions in Germany 160 characters maximum allowed!"]);

                }
            }

            await _unitOfWork.Repository<CourseSeo>().AddAsync(CourseSeo);
                try
                {
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseSeosCacheKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return await Result<int>.SuccessAsync(CourseSeo.Id, _localizer["Course Seo Saved"]);
            
        }
        else
        {
            var CourseSeo = await _unitOfWork.Repository<CourseSeo>().GetByIdAsync(command.Id);
            if (CourseSeo != null)
            {
                CourseSeo.CourseId = command.CourseId;



                CourseSeo.MetaTitleAr = command.MetaTitleAr ?? CourseSeo.MetaTitleAr;
                CourseSeo.MetaTitleEn = command.MetaTitleEn ?? CourseSeo.MetaTitleEn;
                CourseSeo.MetaTitleGe = command.MetaTitleGe ?? CourseSeo.MetaTitleGe;
                
                CourseSeo.MetaNameAr = command.MetaNameAr ?? CourseSeo.MetaNameAr;
                CourseSeo.MetaNameEn = command.MetaNameEn ?? CourseSeo.MetaNameEn;
                CourseSeo.MetaNameGe = command.MetaNameGe ?? CourseSeo.MetaNameGe;
                
                CourseSeo.MetaUrlAr = command.MetaUrlAr ?? CourseSeo.MetaUrlAr;
                CourseSeo.MetaUrlEn = command.MetaUrlEn ?? CourseSeo.MetaUrlEn;
                CourseSeo.MetaUrlGe = command.MetaUrlGe ?? CourseSeo.MetaUrlGe;


                CourseSeo.MetaKeywordsAr = command.MetaKeywordsAr ?? CourseSeo.MetaKeywordsAr;
                CourseSeo.MetaKeywordsEn = command.MetaKeywordsEn ?? CourseSeo.MetaKeywordsEn;
                CourseSeo.MetaKeywordsGe = command.MetaKeywordsGe ?? CourseSeo.MetaKeywordsGe;



                CourseSeo.ImageAlt1Ar = command.ImageAlt1Ar ?? CourseSeo.ImageAlt1Ar;
                CourseSeo.ImageAlt1En = command.ImageAlt1En ?? CourseSeo.ImageAlt1En;
                CourseSeo.ImageAlt1Ge = command.ImageAlt1Ge ?? CourseSeo.ImageAlt1Ge;

                CourseSeo.ImageAlt2Ar = command.ImageAlt2Ar ?? CourseSeo.ImageAlt2Ar;
                CourseSeo.ImageAlt2En = command.ImageAlt2En ?? CourseSeo.ImageAlt2En;
                CourseSeo.ImageAlt2Ge = command.ImageAlt2Ge ?? CourseSeo.ImageAlt2Ge;
                
                CourseSeo.ImageAlt3Ar = command.ImageAlt3Ar ?? CourseSeo.ImageAlt3Ar;
                CourseSeo.ImageAlt3En = command.ImageAlt3En ?? CourseSeo.ImageAlt3En;
                CourseSeo.ImageAlt3Ge = command.ImageAlt3Ge ?? CourseSeo.ImageAlt3Ge;  
                

                CourseSeo.ImageAlt4Ar = command.ImageAlt4Ar ?? CourseSeo.ImageAlt4Ar;
                CourseSeo.ImageAlt4En = command.ImageAlt4En ?? CourseSeo.ImageAlt4En;
                CourseSeo.ImageAlt4Ge = command.ImageAlt4Ge ?? CourseSeo.ImageAlt4Ge;



                if (command.MetaDescriptionsAr != null)
                {
                    if (command.MetaDescriptionsAr.Length > 320)
                    {
                        return await Result<int>.FailAsync(_localizer["Descriptions in Arabic 320 characters maximum allowed!"]);
                    }
                    else
                    {
                        CourseSeo.MetaDescriptionsAr = command.MetaDescriptionsAr ?? CourseSeo.MetaDescriptionsAr;
                    }
                }

                if (command.MetaDescriptionsEn != null)
                {
                    if (command.MetaDescriptionsEn.Length > 160)
                    {
                        return await Result<int>.FailAsync(_localizer["Descriptions in English 160 characters maximum allowed!"]);
                    }
                    else
                    {
                        CourseSeo.MetaDescriptionsEn = command.MetaDescriptionsEn ?? CourseSeo.MetaDescriptionsEn;
                    }
                }

                if (command.MetaDescriptionsGe != null)
                {
                    if (command.MetaDescriptionsGe.Length > 160)
                    {
                        return await Result<int>.FailAsync(_localizer["Descriptions in Germany 160 characters maximum allowed!"]);
                    }
                    else
                    {
                        CourseSeo.MetaDescriptionsGe = command.MetaDescriptionsGe ?? CourseSeo.MetaDescriptionsGe;
                    }
                }




                CourseSeo.MetaDescriptionsAr = command.MetaDescriptionsAr ?? CourseSeo.MetaDescriptionsAr;
                CourseSeo.MetaDescriptionsEn = command.MetaDescriptionsEn ?? CourseSeo.MetaDescriptionsEn;
                CourseSeo.MetaDescriptionsGe = command.MetaDescriptionsGe ?? CourseSeo.MetaDescriptionsGe;

                CourseSeo.MetaRobots = command.MetaRobots ?? CourseSeo.MetaRobots;




              
                await _unitOfWork.Repository<CourseSeo>().UpdateAsync(CourseSeo);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseSeosCacheKey);
                return await Result<int>.SuccessAsync(CourseSeo.Id, _localizer["Course Seo Updated"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Course Seo Not Found!"]);
            }
        }
    }
}
