


public class LobbyService
{
  private readonly ILogger<LobbyService> _logger;

  private static Dictionary<string, Lobby> _lobbies = new Dictionary<string, Lobby>();

  public LobbyService(ILogger<LobbyService> logger)
  {
    _logger = logger;
  }

  public string CreateLobby()
  {
    var lobbyId = Guid.NewGuid().ToString();
    var lobby = new Lobby
    {
      LobbyId = lobbyId,
      Started = false
    };

    _lobbies.Add(lobbyId, lobby);

    return lobbyId;
  }

  public bool JoinLobby(string lobbyId, UserInfo user)
  {
    if (!_lobbies.TryGetValue(lobbyId, out var lobby))
    {
      return false;
    }


    var lobbyUser = new LobbyUser
    {
      UserId = user.UserId,
      SteamId = user.UserId,
      LastUpdate = DateTime.UtcNow
    };

    lobby.UpsertUser(lobbyUser);

    return true;
  }

  public bool RefreshUser(string lobbyId, UserInfo user)
  {

    if (!_lobbies.TryGetValue(lobbyId, out var lobby))
    {
      return false;
    }

    var lobbyUser = new LobbyUser
    {
      UserId = user.UserId,
      SteamId = user.UserId,
      LastUpdate = DateTime.UtcNow
    };

    lobby.RefreshUser(lobbyUser);
    return true;
  }

  public Lobby? GetLobby(string lobbyId)
  {
    return _lobbies.GetValueOrDefault(lobbyId);
  }

  public bool StartLobby(string lobbyId)
  {
    if (!_lobbies.TryGetValue(lobbyId, out var lobby))
    {
      return false;
    }

    lobby.StartLobby();

    return true;
  }

  public IEnumerable<Lobby> GetLobbies()
  {
    return _lobbies.Values;
  }

  public MapVoting? GetMapVoting(string lobbyId)
  {
    if (!_lobbies.TryGetValue(lobbyId, out var lobby))
    {
      _logger.LogInformation($"No lobby with id {lobbyId}");
      return null;
    }

    if (!lobby.Started)
    {

      _logger.LogInformation($"Lobby {lobbyId} not started");
      return null;
    }

    var match = lobby.LobbyMatch;
    if (match == null)
    {
      _logger.LogCritical($"Lobby {lobbyId} started but no match");
      return null;
    }

    return match.GetMapVotingState();
  }

  public void AddVote(string lobbyId, string mapName, LobbyUser user)
  {
    if (!_lobbies.TryGetValue(lobbyId, out var lobby))
    {
      _logger.LogInformation($"No lobby with id {lobbyId}");
      return;
    }

    if (lobby.LobbyMatch == null)
    {
      _logger.LogCritical($"Lobby {lobbyId} started but no match");
      return;
    }

    var mapVote = new MapVote
    {
      MapName = mapName,
      User = user
    };

    lobby.LobbyMatch.AddVote(mapVote);

  }

  public LobbyUser? GetLobbyUser(string lobbyId, string userId)
  {
    if (!_lobbies.TryGetValue(lobbyId, out var lobby))
    {
      _logger.LogInformation($"No lobby with id {lobbyId}");
      return null;
    }

    return lobby.GetLobbyUser(userId);
  }

}