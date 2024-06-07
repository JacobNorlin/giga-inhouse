public class Match
{
  public Lobby SourceLobby { get; set; }

  public List<LobbyUser> T { get; private set; } = new List<LobbyUser>();
  public List<LobbyUser> CT { get; private set; } = new List<LobbyUser>();

  public Match(Lobby lobby)
  {
    SourceLobby = lobby;

    ShuffleTeams();
  }

  public void ShuffleTeams()
  {
    T = new List<LobbyUser>();
    CT = new List<LobbyUser>();

    var rng = new Random();
    foreach (var user in SourceLobby.Users)
    {
      int team = rng.Next(0, 1);
      team = T.Count < 5 ? team : 1;
      team = T.Count < 5 ? team : 0;

      if (team == 0)
      {
        T.Add(user);
      }
      else
      {
        CT.Add(user);
      }

    }
  }


}

public class MatchService
{

  private IDictionary<string, Match> _matches = new Dictionary<string, Match>();

  public Match CreateMatch(Lobby lobby)
  {
    var match = new Match(lobby);

    _matches.Add(lobby.LobbyId, match);

    return match;
  }

  public Match? GetMatch(string lobbyId)
  {
    _matches.TryGetValue(lobbyId, out var match);
    return match;
  }

  public void ShuffleMatchTeams(string lobbyId)
  {
    if (!_matches.TryGetValue(lobbyId, out var match))
    {
      return;
    }

    match.ShuffleTeams();
  }




}