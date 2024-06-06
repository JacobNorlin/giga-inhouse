
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

  public ulong GetNumericSteamId()
  {
    return ulong.Parse(this.SteamId);
  }
}



public class Lobby
{

  public List<LobbyUser> Users { get; set; } = new List<LobbyUser>();
  public bool HasUser(string userId)
  {
    return Users.Any(user =>
    {
      return user.UserId == userId;
    });
  }

  public LobbyUser GetUserBySteamId(ulong steamId)
  {
    var user = Users.Find(user =>
    {
      return user.GetNumericSteamId() == steamId;
    });

    if (user == null)
    {
      throw new Exception($"User with SteamId {steamId} does not exist");
    }

    return user;
  }

}