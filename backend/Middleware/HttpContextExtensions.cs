public static class HttpContextExtensions
{

    public static void SetUser(this HttpContext context, UserInfo user)
    {
        context.Items["user"] = user;
    }

    public static UserInfo GetUser(this HttpContext context)
    {
        var rawUser = context.Items["user"];
        return rawUser as UserInfo ?? throw new Exception("User not defined");
    }

}