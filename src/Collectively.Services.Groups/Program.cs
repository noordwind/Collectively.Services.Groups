using Collectively.Common.Host;
using Collectively.Messages.Commands.Users;
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
                // .SubscribeToCommand<SignUp>()
                // .SubscribeToCommand<SignOut>()
                // .SubscribeToCommand<ChangeUsername>()
                // .SubscribeToCommand<UploadAvatar>()
                // .SubscribeToCommand<RemoveAvatar>()
                // .SubscribeToCommand<ChangePassword>()
                // .SubscribeToCommand<ResetPassword>()
                // .SubscribeToCommand<SetNewPassword>()
                // .SubscribeToCommand<PostOnFacebookWall>()
                .Build()
                .Run();
        }
    }
}
