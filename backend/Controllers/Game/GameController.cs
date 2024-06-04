using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public class GameController : ControllerBase
{

  private readonly CS2Service _cs2Service;
  private readonly ILogger<GameController> _logger;

  public GameController(ILogger<GameController> logger, CS2Service cs2Service)
  {
    _logger = logger;
    _cs2Service = cs2Service;
  }

  [HttpGet()]
  [Route("[controller]/start")]
  public string Get()
  {
    return "Hello";
  }
}
