public static class CookieHelper
{
    public static void SetCookie(HttpContext context, string key, string value, int ? expireTime)
    {
        CookieOptions option = new CookieOptions();

        if (expireTime.HasValue)
            option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
        else
            option.Expires = DateTime.Now.AddDays(7);

        context.Response.Cookies.Append(key, value, option);
    }

    public static string GetCookie(HttpContext context, string key)
    {
        return context.Request.Cookies[key];
    }
}
