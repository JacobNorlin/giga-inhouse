

using backend.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class LobbyController : ControllerBase
{


  private readonly ILogger<LobbyController> _logger;
  private readonly LobbyService _lobbyService;

  public LobbyController(ILogger<LobbyController> logger, LobbyService lobbyService)
  {
    _logger = logger;
    _lobbyService = lobbyService;
  }


  [HttpPost()]
  [Route("[controller]/Create")]
  public IActionResult CreateLobby()
  {
    var lobbyId = _lobbyService.CreateLobby();

    return Ok(lobbyId);
  }

  [HttpPost()]
  [Route("[controller]/Join")]
  public IActionResult JoinLobby([FromQuery] string lobbyId)
  {
    var user = HttpContext.GetUser();

    if (user.SteamId == null)
    {
      return BadRequest(new Error("NoSteamId", "Must set steam id to join lobby"));
    }

    var success = _lobbyService.JoinLobby(lobbyId, user);

    if (!success)
    {
      return BadRequest(new Error("NoSuchLobby", $"Lobby with id {lobbyId} does not exist"));
    }

    return Ok();
  }

  [HttpGet()]
  [Route("[controller]")]
  public IActionResult GetLobby([FromQuery] string lobbyId)
  {
    var lobby = _lobbyService.GetLobby(lobbyId);

    if (lobby == null)
    {
      return BadRequest(new Error("NoSuchLobby", $"Lobby with id {lobbyId} does not exist"));
    }
    var user = HttpContext.GetUser();
    _lobbyService.RefreshUser(lobbyId, user);

    return Ok(lobby);
  }

  [HttpGet()]
  [Route("[controller]/List")]
  public IActionResult GetLobbies()
  {
    return Ok(_lobbyService.GetLobbies());
  }

  [HttpPost()]
  [Route("[controller]/Start")]
  public IActionResult StartLobby([FromQuery] string lobbyId)
  {
    var success = _lobbyService.StartLobby(lobbyId);
    if (!success)
    {
      return BadRequest(new Error("NoSuchLobby", $"Lobby with id {lobbyId} does not exist"));
    }

    return Ok();
  }

  [HttpGet()]
  [Route("[controller]/Vote")]
  public IActionResult GetVoting([FromQuery] string lobbyId)
  {
    var mapVoting = _lobbyService.GetMapVoting(lobbyId);

    if (mapVoting == null)
    {
      return BadRequest();
    }

    return Ok(mapVoting);
  }

  [HttpPost()]
  [Route("[controller]/Vote")]
  public IActionResult GetVoting([FromQuery] string lobbyId, [FromQuery] string mapName)
  {

    var userInfo = HttpContext.GetUser();
    var lobbyUser = _lobbyService.GetLobbyUser(lobbyId, userInfo.UserId);

    if (lobbyUser == null)
    {
      return BadRequest(new Error("UserNotInLobby", "User is not in this lobby"));
    }

    _lobbyService.AddVote(lobbyId, mapName, lobbyUser);

    return Ok();

  }
}