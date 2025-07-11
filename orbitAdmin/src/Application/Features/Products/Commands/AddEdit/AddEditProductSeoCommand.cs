using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Commands.AddEdit
{
    public class AddEditProductSeoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public int ProductId { get; set; }


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

internal class AddEditProductSeoCommandHandler : IRequestHandler<AddEditProductSeoCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditProductSeoCommandHandler> _localizer;
    private readonly IUnitOfWork<int> _unitOfWork;

    public AddEditProductSeoCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditProductSeoCommandHandler> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Result<int>> Handle(AddEditProductSeoCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == 0)
        {

            var productSeo = _mapper.Map<ProductSeo>(command);



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

            await _unitOfWork.Repository<ProductSeo>().AddAsync(productSeo);
                try
                {
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductSeosCacheKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return await Result<int>.SuccessAsync(productSeo.Id, _localizer["Product Seo Saved"]);
            
        }
        else
        {
            var productSeo = await _unitOfWork.Repository<ProductSeo>().GetByIdAsync(command.Id);
            if (productSeo != null)
            {
                productSeo.ProductId = command.ProductId;



                productSeo.MetaTitleAr = command.MetaTitleAr ?? productSeo.MetaTitleAr;
                productSeo.MetaTitleEn = command.MetaTitleEn ?? productSeo.MetaTitleEn;
                productSeo.MetaTitleGe = command.MetaTitleGe ?? productSeo.MetaTitleGe;
                
                productSeo.MetaNameAr = command.MetaNameAr ?? productSeo.MetaNameAr;
                productSeo.MetaNameEn = command.MetaNameEn ?? productSeo.MetaNameEn;
                productSeo.MetaNameGe = command.MetaNameGe ?? productSeo.MetaNameGe;
                
                productSeo.MetaUrlAr = command.MetaUrlAr ?? productSeo.MetaUrlAr;
                productSeo.MetaUrlEn = command.MetaUrlEn ?? productSeo.MetaUrlEn;
                productSeo.MetaUrlGe = command.MetaUrlGe ?? productSeo.MetaUrlGe;


                productSeo.MetaKeywordsAr = command.MetaKeywordsAr ?? productSeo.MetaKeywordsAr;
                productSeo.MetaKeywordsEn = command.MetaKeywordsEn ?? productSeo.MetaKeywordsEn;
                productSeo.MetaKeywordsGe = command.MetaKeywordsGe ?? productSeo.MetaKeywordsGe;



                productSeo.ImageAlt1Ar = command.ImageAlt1Ar ?? productSeo.ImageAlt1Ar;
                productSeo.ImageAlt1En = command.ImageAlt1En ?? productSeo.ImageAlt1En;
                productSeo.ImageAlt1Ge = command.ImageAlt1Ge ?? productSeo.ImageAlt1Ge;

                productSeo.ImageAlt2Ar = command.ImageAlt2Ar ?? productSeo.ImageAlt2Ar;
                productSeo.ImageAlt2En = command.ImageAlt2En ?? productSeo.ImageAlt2En;
                productSeo.ImageAlt2Ge = command.ImageAlt2Ge ?? productSeo.ImageAlt2Ge;
                
                productSeo.ImageAlt3Ar = command.ImageAlt3Ar ?? productSeo.ImageAlt3Ar;
                productSeo.ImageAlt3En = command.ImageAlt3En ?? productSeo.ImageAlt3En;
                productSeo.ImageAlt3Ge = command.ImageAlt3Ge ?? productSeo.ImageAlt3Ge;  
                

                productSeo.ImageAlt4Ar = command.ImageAlt4Ar ?? productSeo.ImageAlt4Ar;
                productSeo.ImageAlt4En = command.ImageAlt4En ?? productSeo.ImageAlt4En;
                productSeo.ImageAlt4Ge = command.ImageAlt4Ge ?? productSeo.ImageAlt4Ge;



                if (command.MetaDescriptionsAr != null)
                {
                    if (command.MetaDescriptionsAr.Length > 320)
                    {
                        return await Result<int>.FailAsync(_localizer["Descriptions in Arabic 320 characters maximum allowed!"]);
                    }
                    else
                    {
                        productSeo.MetaDescriptionsAr = command.MetaDescriptionsAr ?? productSeo.MetaDescriptionsAr;
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
                        productSeo.MetaDescriptionsEn = command.MetaDescriptionsEn ?? productSeo.MetaDescriptionsEn;
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
                        productSeo.MetaDescriptionsGe = command.MetaDescriptionsGe ?? productSeo.MetaDescriptionsGe;
                    }
                }




                productSeo.MetaDescriptionsAr = command.MetaDescriptionsAr ?? productSeo.MetaDescriptionsAr;
                productSeo.MetaDescriptionsEn = command.MetaDescriptionsEn ?? productSeo.MetaDescriptionsEn;
                productSeo.MetaDescriptionsGe = command.MetaDescriptionsGe ?? productSeo.MetaDescriptionsGe;

                productSeo.MetaRobots = command.MetaRobots ?? productSeo.MetaRobots;




              
                await _unitOfWork.Repository<ProductSeo>().UpdateAsync(productSeo);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductSeosCacheKey);
                return await Result<int>.SuccessAsync(productSeo.Id, _localizer["Product Seo Updated"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Seo Not Found!"]);
            }
        }
    }
}
