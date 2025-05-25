using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System;
using static SchoolV01.Shared.Constants.Permission.Permissions;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Application.Requests.Identity;

namespace SchoolV01.Application.Features.Clients.Companies.Commands.Delete
{
    public class DeleteCompanyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCompanyCommandHandler> _localizer;
        private readonly IUserService _userService;
        public DeleteCompanyCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCompanyCommandHandler> localizer, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userService = userService;
        }

        public async Task<Result<int>> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(command.Id);
            var client = await _unitOfWork.Repository<Client>().GetByIdAsync(company.ClientId);
            if (company != null && client != null)
            {
                client.Deleted = true;
                company.Deleted = true;
                client.IsActive = false;

                ToggleUserStatusRequest toggleUserStatusRequest = new ToggleUserStatusRequest();
                toggleUserStatusRequest.ActivateUser = false;
                toggleUserStatusRequest.UserId = client.UserId;
                await _userService.ToggleUserStatusAsync(toggleUserStatusRequest);

                await _unitOfWork.Repository<Company>().DeleteAsync(company);
                await _unitOfWork.Repository<Client>().DeleteAsync(client);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCompaniesCacheKey);
                return await Result<int>.SuccessAsync(company.Id, _localizer["Company Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Company Not Found!"]);
            }
        }
    }
}
