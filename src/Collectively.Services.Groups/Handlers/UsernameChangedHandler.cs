using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Users;
using Collectively.Services.Groups.Services;

namespace Collectively.Services.Groups.Handlers
{
    public class UsernameChangedHandler : IEventHandler<UsernameChanged>
    {
        private readonly IHandler _handler;
        private readonly IUserService _userService;

        public UsernameChangedHandler(IHandler handler, IUserService userService)
        {
            _handler = handler;
            _userService = userService;
        }

        public async Task HandleAsync(UsernameChanged @event)
        => await _handler
            .Run(async () => 
            {
                await _userService.UpdateNameAsync(@event.UserId, @event.NewName);
            })
            .ExecuteAsync();
    }
}