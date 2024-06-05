

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class LobbyController : ControllerBase
{

  private static readonly Lobby _lobby = new Lobby();

  private readonly ILogger<LobbyController> _logger;

  public LobbyController(ILogger<LobbyController> logger)
  {
    _logger = logger;
  }



  [HttpPost()]
  [Route("[controller]/join")]
  public object JoinLobby()
  {

    var user = HttpContext.GetUser();
    _lobby.Users.Append(user);

    return Ok(_lobby);
  }

  [HttpGet()]
  [Route("[controller]")]

  public object GetLobby()
  {
    var user = HttpContext.GetUser();
    if (!_lobby.HasUser(user.UserId!))
    {
      _lobby.Users.Add(user);
    }
    return Ok(_lobby);
  }

}