using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Groups;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Groups.Services;
using RawRabbit;

namespace Collectively.Services.Groups.Handlers
{
    public class CreateGroupHandler : ICommandHandler<CreateGroup>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IGroupService _groupService;

        public CreateGroupHandler(IHandler handler,
            IBusClient bus, 
            IGroupService groupService)
        {
            _handler = handler;
            _bus = bus;
            _groupService = groupService;
        }

        public async Task HandleAsync(CreateGroup command)
        {
            await _handler
                .Run(async () => await _groupService.CreateAsync(command.Name,
                    command.UserId, command.Criteria))
                .OnSuccess(async () => await _bus.PublishAsync(
                    new GroupCreated(command.Request.Id, command.UserId,
                        command.GroupId, command.OrganizationId)))
                .OnCustomError(async ex => await _bus.PublishAsync(
                    new CreateGroupRejected(command.Request.Id, command.UserId,
                        command.GroupId, ex.Code, ex.Message)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, $"Error when trying to create a group: '{command.Name}', id: '{command.GroupId}'.");
                    await _bus.PublishAsync(new CreateGroupRejected(command.Request.Id, command.UserId,
                        command.GroupId, OperationCodes.Error, "Error when trying to create a group."));
                })
                .ExecuteAsync();
        }
    }
}