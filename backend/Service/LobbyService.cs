


public class Lobby : IDisposable
{
  public required string LobbyId { get; set; }
  public required bool Started { get; set; } = false;
  public List<LobbyUser> Users { get; set; } = new List<LobbyUser>();


  private Task _cullTask;

  public Lobby()
  {
    _cullTask = _CullInactiveUsers();
  }


  public void UpsertUser(LobbyUser user)
  {
    var userIdx = Users.FindIndex(user =>
    {
      return user.UserId == user.UserId;
    });

    if (userIdx != -1)
    {
      Users[userIdx] = user;
    }
    else
    {
      Users.Add(user);

    }
  }

  public void RefreshUser(LobbyUser userUpdate)
  {
    var userIdx = Users.FindIndex(user =>
    {
      return user.UserId == user.UserId;
    });

    if (userIdx != -1)
    {
      Users[userIdx] = userUpdate;
    }
  }

  private async Task _CullInactiveUsers()
  {
    using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    while (true)
    {

      var now = DateTime.UtcNow;
      Users = Users.Where(user =>
      {
        var secondsSinceLastUpdate = (now - user.LastUpdate).TotalSeconds;
        return secondsSinceLastUpdate < 10;
      }).ToList();

      await timer.WaitForNextTickAsync();
    }
  }

  public void Dispose()
  {
    _cullTask.Dispose();
  }
}

public class LobbyUser
{
  public required string UserId { get; set; }
  public required string SteamId { get; set; }
  public required DateTime LastUpdate { get; set; }
}


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

    lobby.Started = true;


    return true;
  }

  public IEnumerable<Lobby> GetLobbies()
  {
    return _lobbies.Values;
  }

}