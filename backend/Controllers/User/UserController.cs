using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class UserController : ControllerBase
{

  private ILogger<UserController> _logger;
  private UserService _userService;

  public UserController(ILogger<UserController> logger, UserService userService)
  {
    _logger = logger;
    _userService = userService;
  }


  [HttpPost()]
  [Route("[controller]/register")]
  public IActionResult Register([FromBody] CreateUser createUser)
  {
    if (string.IsNullOrEmpty(createUser.Password) || string.IsNullOrEmpty(createUser.UserId))
    {
      return BadRequest(new Error("MissingArgument", "Missing credentials"));
    }

    var existingUser = _userService.GetUser(createUser.UserId);

    if (existingUser != null)
    {
      return BadRequest(new Error("UserExists", "User already exists"));
    }

    // Hash password 
    var hasher = new PasswordHasher<string>();
    var hashed = hasher.HashPassword(createUser.UserId, createUser.Password);

    _userService.CreateUser(createUser.UserId, hashed);


    return Ok();
  }

  [HttpPost()]
  [Route("[controller]/login")]
  public async Task<IActionResult> Login([FromBody()] LoginUser login)
  {
    var user = _userService.GetUser(login.UserId!);

    if (user == null)
    {
      return Unauthorized(new Error("BadLogin", "Unable to verify username or password"));
    }

    var hasher = new PasswordHasher<string>();
    if (hasher.VerifyHashedPassword(login.UserId!, user.Password!, login.ProvidedPassword!) == PasswordVerificationResult.Failed)
    {
      return Unauthorized(new Error("BadLogin", "Unable to verify username or password"));
    }

    var sessionId = _userService.UpsertSession(login.UserId!);


    var claims = new List<Claim>{
      new Claim(ClaimTypes.Name, user.UserId),
      new Claim(ClaimTypes.Authentication, sessionId)
    };
    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

    return Ok();
  }

  [HttpGet()]
  [Route("[controller]/validate")]
  public object ValidateSessionToken([FromHeader(Name = "Session-Token")] string sessionToken)
  {
    var session = _userService.GetSessionBySessionId(sessionToken);

    if (session == null)
    {
      return Unauthorized();
    }

    return Ok();
  }
}