using SchoolV01.Application.Features.Clients.Companies.Commands.Delete;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Clients.Persons.Commands.Delete;

namespace SchoolV01.Application.Features.Clients.Companies.Commands.RefusePersonRequest
{
    public class RefusePersonRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class RefusePersonRequestCommandHandler : IRequestHandler<RefusePersonRequestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeletePersonCommandHandler> _localizer;

        public RefusePersonRequestCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePersonCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(RefusePersonRequestCommand command, CancellationToken cancellationToken)
        {
            var Person = await _unitOfWork.Repository<Person>().GetByIdAsync(command.Id);
            var client = await _unitOfWork.Repository<Client>().GetByIdAsync(Person.ClientId);
            if (client != null)
            {
                client.Status = ClientStatusEnum.Refused.ToString();
                await _unitOfWork.Repository<Client>().UpdateAsync(client);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPersonsCacheKey);
                return await Result<int>.SuccessAsync(Person.Id, _localizer["Person Refused"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Person Not Found!"]);
            }
        }
    }
}