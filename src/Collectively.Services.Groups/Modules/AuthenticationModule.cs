using Collectively.Common.Security;
using Collectively.Messages.Commands;
using Collectively.Messages.Commands.Users;
using Nancy;

namespace Collectively.Services.Groups.Modules
{
    public class AuthenticationModule : ModuleBase
    {
        public AuthenticationModule(IServiceAuthenticatorHost serviceAuthenticatorHost,
            IJwtTokenHandler jwtTokenHandler) : base(requireAuthentication: false)
        {
            Post("authenticate", args => 
            {
                var credentials = BindRequest<Credentials>();
                var token = serviceAuthenticatorHost.CreateToken(credentials);
                if (token.HasNoValue)
                {
                    return HttpStatusCode.Unauthorized;
                }
                
                return token.Value;
            });
        }        
    }
}