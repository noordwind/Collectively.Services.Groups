namespace Collectively.Services.Groups
{
    public static class OperationCodes
    {
        public static string Success => "success";
        public static string UserNotFound => "user_not_found";
        public static string InactiveUser => "inactive_user";
        public static string GroupNameInUse => "group_name_in_use";
        public static string OrganizationNameInUse => "organization_name_in_use";
        public static string InvalidName => "invalid_name";
        public static string InvalidFile => "invalid_file";
        public static string FileTooBig => "file_too_big";
        public static string TextTooLong => "text_too_long";
        public static string Error => "error";
        public static string InvalidUser => "invalid_user";        
    }
}