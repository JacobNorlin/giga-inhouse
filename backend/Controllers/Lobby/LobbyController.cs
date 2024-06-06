

using backend.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class LobbyController : ControllerBase
{

  private static readonly Lobby _lobby = new Lobby();

  private readonly ILogger<LobbyController> _logger;
  private readonly CS2Service _cs2Service;

  private static Task _lobbyCuller = _CullLobby();

  public LobbyController(ILogger<LobbyController> logger, CS2Service cs2Service)
  {
    _logger = logger;
    _cs2Service = cs2Service;
  }



  [HttpGet()]
  [Route("[controller]")]

  public object GetLobby()
  {
    var user = HttpContext.GetUser();

    if (user.UserId == "giga-admin")
    {
      return Ok(_lobby);
    }

    if (user.SteamId == null)
    {
      return BadRequest(new Error("NoSteamId", "Must have Steam ID configured to join lobby"));
    }
    var lobbyUser = new LobbyUser
    {
      UserId = user.UserId,
      SteamId = user.SteamId,
      Team = CSTeam.T,
      UserName = user.Name,
      LastUpdateTime = DateTime.UtcNow
    };
    //TODO: Add some kind of timer to remove user
    _lobby.AddUser(lobbyUser);

    return Ok(_lobby);
  }

  private static async Task _CullLobby()
  {
    using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    while (true)
    {

      var now = DateTime.UtcNow;
      _lobby.Users = _lobby.Users.Where(user =>
      {
        var secondsSinceLastUpdate = (now - user.LastUpdateTime).TotalSeconds;
        return secondsSinceLastUpdate < 10;
      }).ToList();

      await timer.WaitForNextTickAsync();
    }
  }

  [HttpGet()]
  [Route("[controller]/start")]
  public object StartLobby()
  {

    if (_lobby.IsStarted)
    {
      return BadRequest(new Error("LobbyStarted", "Lobby already started"));
    }

    _logger.LogInformation("Starting lobby");
    _lobby.IsStarted = true;

    _cs2Service.StartGame();

  }

}