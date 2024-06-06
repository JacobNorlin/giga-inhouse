
public enum CSTeam
{
  T = 0,
  CT = 1
}

public class LobbyUser
{
  public required string UserId { get; set; }
  public required string SteamId { get; set; }

  public required CSTeam Team { get; set; }
  public string? UserName { get; set; }

  public required DateTime LastUpdateTime { get; set; }

}


public class Lobby
{
  public List<LobbyUser> Users { get; set; } = new List<LobbyUser>();

  public bool IsStarted {get; set;} = false;

  public void AddUser(LobbyUser user)
  {
    var userIndex = Users.FindIndex(existingUser =>
    {
      return existingUser.UserId == user.UserId;
    });

    if (userIndex != -1)
    {
      Users[userIndex] = user;
    }
    else
    {
      Users.Add(user);
    }
  }

}