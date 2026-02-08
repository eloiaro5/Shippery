using Microsoft.AspNetCore.Mvc;

namespace Shippery.Models.Resources
{
    public class CookieHelper
    {
        public static void SetCookie(Controller c, string name, string value, Microsoft.AspNetCore.Http.CookieOptions options = null)
        {
            value = value.Replace("\n", "~N");
            value = value.Replace(" ", "~W");
            value = value.Replace("!", "~E");
            if (options is null) c.Response.Cookies.Append(name, value);
            else c.Response.Cookies.Append(name, value, options);
        }
    }
}
