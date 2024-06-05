using Microsoft.AspNetCore.Mvc;

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

}