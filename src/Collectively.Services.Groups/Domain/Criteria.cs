using System.Collections.Generic;

namespace Collectively.Services.Groups.Domain
{
    public static class Criteria
    {
        public static class Remark
        {

            public static class Create
            {
                public static string Public => "public";
                public static string Member => "member";
                public static string Moderator => "moderator";
                public static string Administrator => "administrator";
                public static string Owner => "owner";
            }
        
            public static class Resolve
            {
                public static string Public => "public";
                public static string Member => "member";
                public static string Moderator => "moderator";
                public static string Administrator => "administrator";
                public static string Owner => "owner";
            }

            public static ISet<string> Location(params string[] localities) => new HashSet<string>(localities);           
        }         

        public static class Membership
        {
            public static string Public => "public";
            public static string UserRequest => "user_request";
            public static string Invitation => "invitation";
        }
    }
}