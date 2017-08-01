namespace Collectively.Services.Groups.Domain
{
    public static class Criteria
    {
        public static class CreateRemark
        {
            public static string Public => "public";
            public static string Member => "member";
            public static string Moderator => "moderator";
            public static string Administrator => "administrator";
            public static string Owner => "owner";
        } 

        public static class Membership
        {
            public static string Invitation => "invitation";
            public static string UserRequest => "user_request";
            public static string None => "none";
        }
    }
}