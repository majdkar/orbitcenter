using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Models;
using SchoolV01.Shared.Wrapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
 using  ClientNameSpace =  SchoolV01.Domain.Entities.Clients;
using SchoolV01.Core.Entities;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Interfaces.Services.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using SchoolV01.Domain.Contracts;
using SchoolV01.Shared.Constants.Role;


namespace SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit
{
    public class AddEditPersonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        

        public int CountryId { get; set; } = 0;

        public int CityId { get; set; } = 0;

        public string Sex { get; set; }



        public string PersomImageUrl { get; set; }
        public UploadRequest PersomImageUploadRequest { get; set; }

        public string CvFileUrl { get; set; }
        public UploadRequest CvFileUploadRequest { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Qualification { get; set; }
        public string Job { get; set; }
        public int? ClassificationId { get; set; }

        public string Fax { get; set; }
        public string MailBox { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public string AdditionalInfo { get; set; }

        public string UserId { get; set; }
        public string Status { get; set; }

      
    }
    internal class AddEditPersonCommandHandler : IRequestHandler<AddEditPersonCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditPersonCommandHandler> _localizer;
        private readonly IUserService _userService;
        private readonly IAccountService _AccountService;

        public AddEditPersonCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper,IUploadService uploadService, IStringLocalizer<AddEditPersonCommandHandler> localizer, IUserService userService, IAccountService AccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _userService = userService;
            _AccountService = AccountService;
        }

        public async Task<Result<int>> Handle(AddEditPersonCommand command, CancellationToken cancellationToken)
        {
            var PersomImageUploadRequest = command.PersomImageUploadRequest;
            var CvFileUploadRequest = command.CvFileUploadRequest;

        

            if (PersomImageUploadRequest != null)
            {
                PersomImageUploadRequest.FileName = $"{Path.GetRandomFileName()}{PersomImageUploadRequest.Extension}";
            }

            if (CvFileUploadRequest != null)
            {
                CvFileUploadRequest.FileName = $"{Path.GetRandomFileName()}{CvFileUploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                //create client
                var client = new Client
                {
                    Status = command.Status ?? ClientStatusEnum.Pending.ToString(),
                    Type = ClientTypesEnum.Person.ToString(),
                    UserId = command.UserId,
                    
                };

                await _unitOfWork.Repository<ClientNameSpace.Client>().AddAsync(client);
                await _unitOfWork.Commit(cancellationToken);
                //create person
                var person = _mapper.Map<Person>(command);
                person.ClientId = client.Id;
                person.CountryId = person.CountryId == 0 ? null : person.CountryId;
                person.CityId = command.CityId == 0 ? null : command.CityId;
             
                if (PersomImageUploadRequest != null)
                {
                    person.PersomImageUrl = _uploadService.UploadAsync(PersomImageUploadRequest);
                }
                if (CvFileUploadRequest != null)
                {
                    person.CvFileUrl = _uploadService.UploadAsync(CvFileUploadRequest);
                }
                await _unitOfWork.Repository<Person>().AddAsync(person);
                await _unitOfWork.Commit(cancellationToken);
                if (person.Id > 0)
                {
                    return await Result<int>.SuccessAsync(person.Id, _localizer["Person Saved"]);
                }
                else
                {
                    ToggleUserStatusRequest toggleUserStatusRequest = new ToggleUserStatusRequest();
                    toggleUserStatusRequest.ActivateUser = false;
                    toggleUserStatusRequest.UserId = command.UserId;
                    await _userService.ToggleUserStatusAsync(toggleUserStatusRequest); 
                    return await Result<int>.FailAsync("Person Not saved and user inactivated");
                }
                
            }
            else
            {
                var person = await _unitOfWork.Repository<Person>().GetByIdAsync(command.Id);
                if (person != null)
                {
                    person.FullName = command.FullName ?? person.FullName;
                    person.Mobile1 = command.Mobile1 ?? person.Mobile1;
                    person.Mobile2 = command.Mobile2 ?? person.Mobile2;
                    person.Qualification = command.Qualification ?? person.Qualification;
                    person.Job = command.Job ?? person.Job;
                    person.CountryId = (command.CountryId == 0) ? person.CountryId : command.CountryId;

                    person.CityId = (command.CityId == 0) ? person.CityId : command.CityId;
                    person.ClassificationId = (command.ClassificationId == 0) ? person.ClassificationId : command.ClassificationId;

                    person.BirthDate = command.BirthDate ?? person.BirthDate;
                    person.Sex = command.Sex ?? person.Sex;
                    person.Phone = command.Phone ?? person.Phone;
                    person.Fax = command.Fax ?? person.Fax;
                    person.MailBox = command.MailBox ?? person.MailBox;
                    person.Address = command.Address ?? person.Address;
                    person.AdditionalInfo = command.AdditionalInfo ?? person.AdditionalInfo;

                    //Update Email
                    //if (person.Email != command.Email && !String.IsNullOrEmpty(command.Email))
                    //{
                    //    await _AccountService.UpdateEmailByAdminAsync(command.UserId, command.Email);
                    //    person.Email = command.Email;
                    //}
             
                    if (PersomImageUploadRequest != null)
                    {
                        person.PersomImageUrl = _uploadService.UploadAsync(PersomImageUploadRequest);
                    }
                    if (CvFileUploadRequest != null)
                    {
                        person.CvFileUrl = _uploadService.UploadAsync(CvFileUploadRequest);
                    }

                    await _unitOfWork.Repository<Person>().UpdateAsync(person);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(person.Id, _localizer["Person Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Person Not Found!"]);
                }
            }
        }
    }
}