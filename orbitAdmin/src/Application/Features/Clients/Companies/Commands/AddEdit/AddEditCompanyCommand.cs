using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Wrapper;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Application.Interfaces.Services.Account;


namespace SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit
{
    public class AddEditCompanyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public int CountryId { get; set; }
        public int CityId { get; set; } 

        public string Phone { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string CompanyImageUrl { get; set; }
        public string CompanyFileUrl { get; set; }

        public DateTime? LicenseIssuingDate { get; set; }


        public string ResponsiblePersonNameAr { get; set; }
        public string ResponsiblePersonNameEn { get; set; }
        public string ResponsiblePersonMobile { get; set; }

        public string AdditionalInfo { get; set; }


        public UploadRequest CompanyImageUploadRequest { get; set; }
        public UploadRequest CompanyFileUploadRequest { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; } = ClientStatusEnum.Pending.ToString();

    }

    internal class AddEditCompanyCommandHandler : IRequestHandler<AddEditCompanyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditCompanyCommandHandler> _localizer;
        private readonly IUserService _userService;
        private readonly IAccountService _AccountService;

        public AddEditCompanyCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditCompanyCommandHandler> localizer, IUserService userService, IAccountService AccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _userService = userService;
            _AccountService = AccountService;
        }

        public async Task<Result<int>> Handle(AddEditCompanyCommand command, CancellationToken cancellationToken)
        {
            var CompanyImageUploadRequest = command.CompanyImageUploadRequest;
            var CompanyFileUploadRequest = command.CompanyFileUploadRequest;

            if (CompanyImageUploadRequest != null)
            {
                CompanyImageUploadRequest.FileName = $"{Path.GetRandomFileName()}{CompanyImageUploadRequest.Extension}";
            }

            if (CompanyFileUploadRequest != null)
            {
                CompanyFileUploadRequest.FileName = $"{Path.GetRandomFileName()}{CompanyFileUploadRequest.Extension}";
            }

            if (command.Id == 0) // Add Command
            {

                //create client
                var client = new Client
                {
                    Status = ClientStatusEnum.Accepted.ToString(),
                    Type = ClientTypesEnum.Company.ToString(),
                    UserId = command.UserId,
                };
                await _unitOfWork.Repository<Client>().AddAsync(client);
                await _unitOfWork.Commit(cancellationToken);
                //create company
                var company = _mapper.Map<Company>(command);
                company.ClientId = client.Id;
                if (CompanyImageUploadRequest != null)
                {
                    company.CompanyImageUrl = _uploadService.UploadAsync(CompanyImageUploadRequest);
                }
                if (CompanyFileUploadRequest != null)
                {
                    company.CompanyFileUrl = _uploadService.UploadAsync(CompanyFileUploadRequest);
                }
                company.CountryId = command.CountryId == 0 ? null : command.CountryId;
                company.CityId = command.CityId == 0 ? null : command.CityId;
                await _unitOfWork.Repository<Company>().AddAsync(company);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(company.Id, _localizer["Company Saved"]);

            }

            else // Update Command
            {
                var company = await _unitOfWork.Repository<Company>().GetByIdAsync(command.Id);
                if (company != null)
                {
                    if (command.Status == ClientStatusEnum.Refused.ToString())
                    {
                        command.Status = ClientStatusEnum.Pending.ToString();
                    }
                    //upload files
                    if (CompanyImageUploadRequest != null)
                    {
                        company.CompanyImageUrl = _uploadService.UploadAsync(CompanyImageUploadRequest);
                    }
                    if (CompanyFileUploadRequest != null)
                    {
                        company.CompanyFileUrl = _uploadService.UploadAsync(CompanyFileUploadRequest);
                    }

                    //Update Email
                    //if (company.Email != command.Email)
                    //{
                    //    await _AccountService.UpdateEmailByAdminAsync(command.UserId, command.Email);
                    //    company.Email = command.Email;
                    //}
                    company.NameEn = command.NameEn ?? company.NameEn;
                    company.NameAr = command.NameAr ?? company.NameAr;
                    //company.CountryId = command.CountryId == 0 ? company.CountryId : command.CountryId;
                    company.CountryId = (command.CountryId == 0) ? company.CountryId : command.CountryId;
                    company.CityId = (command.CityId == 0) ? company.CityId : command.CityId;
                    //company.CityName = command.CityName ?? company.CityName;
                    company.Phone = command.Phone ?? company.Phone;
                    company.Address = command.Address ?? company.Address;
                    company.Website = command.Website ?? company.Website;
                    company.LicenseIssuingDate = command.LicenseIssuingDate ?? company.LicenseIssuingDate;
                    company.ResponsiblePersonNameEn = command.ResponsiblePersonNameEn ?? company.ResponsiblePersonNameEn;
                    company.ResponsiblePersonNameAr = command.ResponsiblePersonNameAr ?? company.ResponsiblePersonNameAr;
                    company.ResponsiblePersonMobile = command.ResponsiblePersonMobile ?? company.ResponsiblePersonMobile;
                    company.AdditionalInfo = command.AdditionalInfo ?? company.AdditionalInfo;
                    await _unitOfWork.Repository<Company>().UpdateAsync(company);
                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(company.Id, _localizer["Company Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Company Not Found!"]);
                }
            }
        }
    }
}

