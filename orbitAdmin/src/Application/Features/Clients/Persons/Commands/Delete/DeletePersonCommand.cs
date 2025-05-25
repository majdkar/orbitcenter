using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;
using ClientNameSpace = SchoolV01.Domain.Entities.Clients;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Application.Requests.Identity;

using SchoolV01.Domain.Entities.Clients;

namespace SchoolV01.Application.Features.Clients.Persons.Commands.Delete
{
    public class DeletePersonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeletePersonCommandHandler> _localizer;
        private readonly IUserService _userService;

        public DeletePersonCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePersonCommandHandler> localizer, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userService = userService;
        }

        public async Task<Result<int>> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.Repository<Person>().GetByIdAsync(command.Id);
            var client = await _unitOfWork.Repository<ClientNameSpace.Client>().GetByIdAsync(person.ClientId);
            if (person != null && client!=null)
            {
                
                client.Deleted = true;
                person.Deleted = true;
                client.IsActive = false;

                ToggleUserStatusRequest toggleUserStatusRequest = new ToggleUserStatusRequest();
                toggleUserStatusRequest.ActivateUser = false;
                toggleUserStatusRequest.UserId = client.UserId;
                await _userService.ToggleUserStatusAsync(toggleUserStatusRequest);


                await _unitOfWork.Repository<Person>().DeleteAsync(person);
                await _unitOfWork.Repository<ClientNameSpace.Client>().DeleteAsync(client);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPersonsCacheKey);
                return await Result<int>.SuccessAsync(person.Id, _localizer["Person Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Person Not Found!"]);
            }
        }
    }
}