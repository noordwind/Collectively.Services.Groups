using Collectively.Common.Host;
using Collectively.Messages.Commands.Groups;
using Collectively.Messages.Commands.Users;
using Collectively.Messages.Events.Users;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(args: args)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<CreateGroup>()
                .SubscribeToCommand<CreateOrganization>()
                .SubscribeToCommand<AddMemberToGroup>()
                .SubscribeToCommand<AddMemberToOrganization>()
                .SubscribeToEvent<SignedUp>()
                .SubscribeToEvent<UsernameChanged>()
                .SubscribeToEvent<AccountDeleted>()
                .Build()
                .Run();
        }
    }
}
