using Collectively.Common.Extensions;

namespace Collectively.Services.Groups.Framework
{
    public static class Extensions
    {
        public static string ToCodename(this string value)
        => value.Empty() ? string.Empty : value.TrimToLower().Replace(" ", "-");
    }
}