using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Clients.Companies.Commands.Delete;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Clients.Companies.Commands.AcceptCompanyRequest
{
    public class AcceptCompanyRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class AcceptCompanyRequestCommandHandler : IRequestHandler<AcceptCompanyRequestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCompanyCommandHandler> _localizer;

        public AcceptCompanyRequestCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCompanyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AcceptCompanyRequestCommand command, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(command.Id);
            var client = await _unitOfWork.Repository<Client>().GetByIdAsync(company.ClientId);
            if (client != null)
            {
                client.Status = ClientStatusEnum.Accepted.ToString();
                await _unitOfWork.Repository<Client>().UpdateAsync(client);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCompaniesCacheKey);
                return await Result<int>.SuccessAsync(company.Id, _localizer["Company Accepted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Company Not Found!"]);
            }
        }
    }
}