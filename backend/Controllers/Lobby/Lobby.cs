public class Lobby
{
  public string LobbyId { get; set; }

  public List<UserInfo> Users { get; set; } = new List<UserInfo>();
  public bool HasUser(string userId)
  {
    return Users.Any(user =>
    {
      return user.UserId == userId;
    });
  }

}