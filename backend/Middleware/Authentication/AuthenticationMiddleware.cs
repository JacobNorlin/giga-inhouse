// Middleware to manage validation session claims and amending requests
// with user info

using System.Security.Claims;

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

    // I think the framework already validates the session cookie
    // so not entirely sure this is necessary :^)
    var sessionToken = context.User.FindFirstValue(ClaimTypes.Authentication);

    if (sessionToken == null)
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("No session");
      return;
    }


    var user = _userService.GetUserBySessionId(sessionToken);
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