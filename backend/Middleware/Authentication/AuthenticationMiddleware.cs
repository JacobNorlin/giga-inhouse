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

    if (!context.Request.Headers.TryGetValue("session-token", out var token))
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Missing session token");
      return;
    }

    bool isValidToken = _userService.ValidateSession(token!);

    if (!isValidToken)
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Invalid session token");
      return;
    }

    await _next(context);
  }
}