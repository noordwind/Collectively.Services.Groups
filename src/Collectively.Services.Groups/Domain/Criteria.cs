namespace Collectively.Services.Groups.Domain
{
    public static class Criteria
    {
        public static class AddRemark
        {
            public static string Member => "member";
            public static string Anyone => "anyone";
        } 

        public static class Membership
        {
            public static string Invitation => "invitation";
            public static string UserRequest => "user_request";
            public static string None => "none";
        }
    }
}