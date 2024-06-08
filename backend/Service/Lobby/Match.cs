

using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using giga_inhouse.maps;

public record MapVote
{
  public required LobbyUser User { get; init; }
  public required string MapName { get; init; }
}

public record MapVoting
{
  public required IEnumerable<MapVote> Votes { get; init; }
  public required IEnumerable<string> BannedMaps { get; init; }
  public required IEnumerable<CSMap> Maps { get; init; }
}



public class Match
{
  public IEnumerable<LobbyUser> Users { get; set; }

  public List<LobbyUser> T { get; private set; } = new List<LobbyUser>();
  public List<LobbyUser> CT { get; private set; } = new List<LobbyUser>();

  private List<MapVote> _votes = new List<MapVote>();
  private Dictionary<string, int> _votesPerUser = new Dictionary<string, int>();

  private List<string> _bannedMaps = new List<string>();


  public Match(Lobby lobby)
  {
    Users = lobby.Users;

    ShuffleTeams();
  }

  public void ShuffleTeams()
  {
    T = new List<LobbyUser>();
    CT = new List<LobbyUser>();

    var rng = new Random();
    foreach (var user in Users)
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


  public void AddVote(MapVote vote)
  {
    var voteUserId = vote.User.UserId;

    // Remove vote if user is voting on the same map
    int voteIdx = _votes.FindIndex(v => v.User.UserId == vote.User.UserId);
    if (voteIdx != -1)
    {
      _votes.RemoveAt(voteIdx);
      _votesPerUser[vote.User.UserId] = _votesPerUser[vote.User.UserId] + 1;
      
      return;
    }

    int userHasVotes = _votesPerUser.GetValueOrDefault(vote.User.UserId);
    if (userHasVotes == 0)
    {
      return;
    }

    _votesPerUser[vote.User.UserId] = userHasVotes - 1;
    _votes.Add(vote);
  }


  private void _ResolvingVotingRound()
  {
    List<string> mapsToBan = _votes.GroupBy(v => v.MapName)
      .Select(group =>
      {
        return new
        {
          Count = group.Count(),
          MapName = group.Key
        };
      }).OrderByDescending(v =>
      {
        return v.Count;
      })
      .Select(v => v.MapName)
      .Take(3).ToList();

    _bannedMaps.AddRange(mapsToBan);
  }

  private void _SetVotesPerUser(int numVotes)
  {
    _votesPerUser = new Dictionary<string, int>();
    foreach (var user in Users)
    {
      _votesPerUser.Add(user.UserId, numVotes);
    }
  }

  private async Task _RunMapVoting()
  {
    // Maybe this works, maybe it's a giant mess of race conditions :^)
    var msPerVoteRound = 2000000;



    // Wait for first voting round
    _SetVotesPerUser(1);
    Console.WriteLine("Voting first round");
    await Task.Delay(msPerVoteRound);
    Console.WriteLine("Voting second round");
    _ResolvingVotingRound();
    _SetVotesPerUser(1);
    await Task.Delay(msPerVoteRound);
    _SetVotesPerUser(1);
    Console.WriteLine("Voting third round");
    _ResolvingVotingRound();
    await Task.Delay(msPerVoteRound);
  }


  public async Task StartVoting()
  {
    await _RunMapVoting();
  }

  public MapVoting GetMapVotingState()
  {
    return new MapVoting
    {
      BannedMaps = _bannedMaps,
      Maps = MapPool.Maps,
      Votes = _votes
    };
  }
}