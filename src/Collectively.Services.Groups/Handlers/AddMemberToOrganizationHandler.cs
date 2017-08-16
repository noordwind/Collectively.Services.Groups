using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Groups;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Groups.Services;
using RawRabbit;

namespace Collectively.Services.Groups.Handlers
{
    public class AddMemberToOrganizationHandler : ICommandHandler<AddMemberToOrganization>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IOrganizationService _organizationService;

        public AddMemberToOrganizationHandler(IHandler handler,
            IBusClient bus, IOrganizationService organizationService)
        {
            _handler = handler;
            _bus = bus;
            _organizationService = organizationService;
        }

        public async Task HandleAsync(AddMemberToOrganization command)
        => await _handler
            .Run(async () => await _organizationService.AddMemberAsync(command.OrganizationId, 
                command.MemberId, command.Role))
            .OnSuccess(async () =>
            {
                await _bus.PublishAsync(new MemberAddedToOrganization(command.Request.Id, 
                    command.UserId, command.OrganizationId, command.MemberId, command.Role));
            })
            .OnCustomError(async ex => await _bus.PublishAsync(
                new AddMemberToOrganizationRejected(command.Request.Id, command.UserId,
                    command.OrganizationId, command.MemberId, command.Role, ex.Code, ex.Message)))
            .OnError(async (ex, logger) =>
            {
                var message = $"Error when trying to add member: '{command.MemberId}' to the organization: '{command.OrganizationId}'.";
                logger.Error(ex, message);
                await _bus.PublishAsync(new AddMemberToOrganizationRejected(command.Request.Id, command.UserId,
                    command.OrganizationId, command.MemberId, command.Role, OperationCodes.Error, message));
            })
            .ExecuteAsync();
    }
}