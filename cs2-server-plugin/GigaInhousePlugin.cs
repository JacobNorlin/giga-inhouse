using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace giga_inhouse_plugin;



public class GigaInhousePlugin : BasePlugin
{

  private readonly GigaInhouseServerService _serverService = new GigaInhouseServerService();
  public override string ModuleName => "Giga inhouse";
  public override string ModuleVersion => "0.0.1";


  private readonly ILogger<GigaInhousePlugin> _logger;


  private static Lobby? _sessionLobby;

  public GigaInhousePlugin(ILogger<GigaInhousePlugin> logger)
  {
    _logger = logger;
  }

  public override void Load(bool hotReload)
  {
    _logger.LogInformation("Loading giga inhouse plugin");
  }


  [ConsoleCommand("register_lobby", "Register lobby data")]
  public async void OnRegisterSession(CCSPlayerController? player, CommandInfo info)
  {

    _sessionLobby = await _serverService.GetSessionLobby();
    _logger.LogInformation($"Registered lobby with {_sessionLobby.Users.Count} players");
  }


  [GameEventHandler]
  public HookResult OnPlayerConnect(EventPlayerConnectFull @event, GameEventInfo info)
  {
    if (@event == null)
    {
      _logger.LogInformation("Event was null");
      return HookResult.Continue;
    }
    if (@event.Userid == null)
    {
      _logger.LogInformation("Userid was null");
      return HookResult.Continue;
    }
    if (!@event.Userid.IsValid)
    {
      _logger.LogInformation("Userid not valid");
      return HookResult.Continue;
    }
    if (@event.Userid.IsBot)
    {
      return HookResult.Continue;
    }
    if (_sessionLobby == null)
    {
      throw new Exception("Player connected before lobby was registered");
    }
    var eventUser = @event.Userid;

    if (eventUser == null)
    {
      _logger.LogWarning("Connect event with no user");
      return HookResult.Continue;
    }

    _logger.LogInformation($"Found steam id {eventUser.SteamID}");


    var lobbyUser = _sessionLobby.GetUserBySteamId(eventUser.SteamID);
    _logger.LogInformation("Found user matching steam id");

    switch (lobbyUser.Team)
    {
      case CSTeam.T:
        _logger.LogInformation($"Assigning {lobbyUser.UserId} to T");
        eventUser.ChangeTeam(CsTeam.Terrorist);
        break;
      case CSTeam.CT:
        _logger.LogInformation($"Assigning {lobbyUser.UserId} to CT");
        eventUser.ChangeTeam(CsTeam.CounterTerrorist);
        break;
    }

    return HookResult.Continue;
  }


}
