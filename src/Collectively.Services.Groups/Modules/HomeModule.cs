using System.Collections.Generic;

namespace Collectively.Services.Groups.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule() : base(requireAuthentication: false)
        {
            Get("", args => "Welcome to the Collectively.Services.Groups API!");
            Get("test", args => 
            {
                var result = Domain.Criteria.MergeForGroupOrFail(new Dictionary<string, ISet<string>>
                {
                    ["membership"] = new HashSet<string>{"invitation"}
                });

                return new {result};
            });
        }
    }
}