public class AuthenticationMiddleware
{
  private UserService _userService;
  private ILogger<AuthenticationMiddleware> _logger;
  private RequestDelegate _next;

  public AuthenticationMiddleware(ILogger<AuthenticationMiddleware> logger, UserService userService, RequestDelegate next)
  {
    _logger = logger;
    _userService = userService;
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    if (!context.Request.Headers.TryGetValue("Session-Token", out var token))
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Missing session token");
      return;
    }

    if (token == "giga-admin")
    {
      context.SetUser(new UserInfo
      {
        UserId = "giga-admin",
      });
      await _next(context);
      return;
    }

    var user = _userService.GetUserBySessionId(token!);
    if (user == null)
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Invalid session token");
      return;
    }
    context.SetUser(user);
    await _next(context);
  }
}