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
  private readonly ConsoleCommandService _consoleCommandService;


  private Lobby? _sessionLobby;

  public GigaInhousePlugin(ILogger<GigaInhousePlugin> logger)
  {
    _logger = logger;
    _consoleCommandService = new ConsoleCommandService();
  }

  public override void Load(bool hotReload)
  {
    _logger.LogInformation("Loading giga inhouse plugin");

    _consoleCommandService.Register(this);
  }


  [ConsoleCommand("register_session")]
  public async void OnRegisterSession(CCSPlayerController? player)
  {
    if (player != null)
    {
      return;
    }

    _sessionLobby = await _serverService.GetSessionLobby();
  }


  [GameEventHandler]
  public HookResult OnPlayerConnect(EventPlayerConnect evt, GameEventInfo info)
  {
    if (_sessionLobby == null)
    {
      throw new Exception("Player connected before lobby was registered");
    }
    var eventUser = evt.Userid;

    if (eventUser == null)
    {
      _logger.LogWarning("Connect event with no user");
      return HookResult.Continue;
    }

    var lobbyUser = _sessionLobby.GetUserBySteamId(eventUser.SteamID);

    switch (lobbyUser.Team)
    {
      case CSTeam.T:
        eventUser.ChangeTeam(CsTeam.Terrorist);
        break;
      case CSTeam.CT:
        eventUser.ChangeTeam(CsTeam.CounterTerrorist);
        break;
    }

    return HookResult.Continue;
  }


}
