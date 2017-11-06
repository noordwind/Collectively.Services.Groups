using System.Threading.Tasks;
using Collectively.Common.ServiceClients;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Users;
using Collectively.Services.Groups.Dto;
using Collectively.Services.Groups.Services;

namespace Collectively.Services.Groups.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly IHandler _handler;
        private readonly IUserService _userService;
        private readonly IServiceClient _serviceClient;

        public SignedUpHandler(IHandler handler, IUserService userService, 
            IServiceClient serviceClient)
        {
            _handler = handler;
            _userService = userService;
            _serviceClient = serviceClient;
        }

        public async Task HandleAsync(SignedUp @event)
        => await _handler
            .Run(async () => 
            {
                var user = await _serviceClient.GetAsync<UserDto>(@event.Resource);
                await _userService.CreateIfNotFoundAsync(@event.UserId, 
                    user.Value.Name, user.Value.Role);
            })
            .OnError((ex, logger) =>
            {
                logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
            })
            .ExecuteAsync();
    }
}