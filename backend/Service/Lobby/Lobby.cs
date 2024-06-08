
public class LobbyUser
{
  public required string UserId { get; set; }
  public required string SteamId { get; set; }
  public required DateTime LastUpdate { get; set; }
}


public class Lobby : IDisposable
{
  public required string LobbyId { get; set; }
  public required bool Started { get; set; } = false;
  public List<LobbyUser> Users { get; set; } = new List<LobbyUser>();

  public Match? LobbyMatch { get; set; }


  private Task _cullTask;
  private CancellationTokenSource _cancelCull;

  public Lobby()
  {
    _cancelCull = new CancellationTokenSource();
    _cullTask = _CullInactiveUsers(_cancelCull.Token);
  }


  public void UpsertUser(LobbyUser user)
  {
    var userIdx = Users.FindIndex(existingUser =>
    {
      return existingUser.UserId == user.UserId;
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

  public LobbyUser? GetLobbyUser(string userId)
  {
    return Users.Find(lobbyUser =>
    {
      return lobbyUser.UserId == userId;
    });
  }

  public void RefreshUser(LobbyUser userUpdate)
  {
    var userIdx = Users.FindIndex(user =>
    {
      return userUpdate.UserId == user.UserId;
    });

    if (userIdx != -1)
    {
      Users[userIdx] = userUpdate;
    }
  }

  private async Task _CullInactiveUsers(CancellationToken ct)
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

      if (ct.IsCancellationRequested)
      {
        ct.ThrowIfCancellationRequested();
      }

      await timer.WaitForNextTickAsync();
    }
  }

  public async void StartLobby()
  {
    if (Started)
    {
      return;
    }

    Started = true;

    try
    {
      _cancelCull.Cancel();
      await _cullTask;
    }
    catch (OperationCanceledException _ex)
    {
      Console.WriteLine("Cancelled cull task");
    }
    finally
    {
      _cancelCull.Dispose();
    }

    LobbyMatch = new Match(this);
    await LobbyMatch.StartVoting();
  }

  public void Dispose()
  {
    _cullTask.Dispose();
  }
}
