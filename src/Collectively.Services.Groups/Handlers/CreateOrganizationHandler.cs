using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Groups;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Groups.Services;
using RawRabbit;

namespace Collectively.Services.Groups.Handlers
{
    public class CreateOrganizationHandler : ICommandHandler<CreateOrganization>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IGroupService _groupService;

        public CreateOrganizationHandler(IHandler handler,
            IBusClient bus, 
            IGroupService groupService)
        {
            _handler = handler;
            _bus = bus;
            _groupService = groupService;
        }

        public async Task HandleAsync(CreateOrganization command)
        {
            await _handler
                .Run(async () => await _groupService.CreateAsync(command.Name,
                    command.UserId, command.Criteria))
                .OnSuccess(async () => await _bus.PublishAsync(
                    new OrganizationCreated(command.Request.Id, command.UserId,
                        command.OrganizationId)))
                .OnCustomError(async ex => await _bus.PublishAsync(
                    new CreateOrganizationRejected(command.Request.Id, command.UserId,
                        command.OrganizationId, ex.Code, ex.Message)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, $"Error when trying to create an organization: '{command.Name}', id: '{command.OrganizationId}'.");
                    await _bus.PublishAsync(new CreateOrganizationRejected(command.Request.Id, command.UserId,
                        command.OrganizationId, OperationCodes.Error, "Error when trying to create an organization."));
                })
                .ExecuteAsync();
        }
    }
}