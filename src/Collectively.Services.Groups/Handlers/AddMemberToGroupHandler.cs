using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Groups;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Groups.Services;
using RawRabbit;

namespace Collectively.Services.Groups.Handlers
{
    public class AddMemberToGroupHandler : ICommandHandler<AddMemberToGroup>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IGroupService _groupService;
        private readonly IResourceFactory _resourceFactory;

        public AddMemberToGroupHandler(IHandler handler,
            IBusClient bus,  IGroupService groupService)
        {
            _handler = handler;
            _bus = bus;
            _groupService = groupService;
        }

        public async Task HandleAsync(AddMemberToGroup command)
        => await _handler
            .Run(async () => await _groupService.AddMemberAsync(command.GroupId, 
                command.MemberId, command.Role))
            .OnSuccess(async () =>
            {
                await _bus.PublishAsync(new MemberAddedToGroup(command.Request.Id, 
                    command.UserId, command.GroupId, command.MemberId, command.Role));
            })
            .OnCustomError(async ex => await _bus.PublishAsync(
                new AddMemberToGroupRejected(command.Request.Id, command.UserId,
                    command.GroupId, command.MemberId, command.Role, ex.Code, ex.Message)))
            .OnError(async (ex, logger) =>
            {
                var message = $"Error when trying to add member: '{command.MemberId}' to the group: '{command.GroupId}'.";
                logger.Error(ex, message);
                await _bus.PublishAsync(new AddMemberToGroupRejected(command.Request.Id, command.UserId,
                    command.GroupId, command.MemberId, command.Role, OperationCodes.Error, message));
            })
            .ExecuteAsync();
    }
}