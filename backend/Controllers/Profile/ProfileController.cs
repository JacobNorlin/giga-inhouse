using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public class ProfileController : ControllerBase
{


  private ILogger<ProfileController> _logger;
  private UserService _userService;

  public ProfileController(ILogger<ProfileController> logger, UserService userService)
  {
    _logger = logger;
    _userService = userService;
  }


  [HttpPost()]
  [Route("[controller]")]
  public object UpdateUser([FromBody] UpdateUser updateUser)
  {
    var user = HttpContext.GetUser();

    var updateInfo = new UserInfo
    {
      UserId = user.UserId,
      SteamId = updateUser.SteamId,
      Name = updateUser.UserName

    };

    _userService.UpdateUser(updateInfo);

    return Ok();
  }

  [HttpGet()]
  [Route("[controller]")]
  public IActionResult GetUser()
  {
    var user = HttpContext.GetUser();
    return Ok(user);
  }

}