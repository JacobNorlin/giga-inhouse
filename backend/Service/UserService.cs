
public class UserService
{

  private ILogger<UserService> _logger;
  private DbService _dbService;

  public UserService(ILogger<UserService> logger, DbService dbService)
  {
    _logger = logger;
    _dbService = dbService;
  }

  public User? GetUser(string userId)
  {
    var user = this._dbService.Query<User>(@"
      SELECT * from users
      WHERE userId = @User
    ", new { User = userId });

    return user.FirstOrDefault();
  }

  public object CreateUser(string userId, string hashedPassword)
  {
    var res = _dbService.Query<object>(@"
      INSERT INTO users (userId, name, password)
      VALUES (@UserId, @UserId, @HashedPassword)
    ", new
    {
      UserId = userId,
      HashedPassword = hashedPassword
    }
    );


    return res;
  }

  public string? GetSessionByUserId(string userId)
  {
    var res = _dbService.Query<string>(@"
        SELECT sessionId FROM sessions
        WHERE userId = @UserId
      ", new
    {
      UserId = userId
    });

    return res.FirstOrDefault();
  }

  public string? GetSessionBySessionId(string sessionId)
  {
    var res = _dbService.Query<string>(@"
      SELECT sessionId FROM sessions
      WHERE sessionId = @SessionId
    ", new
    {
      SessionId = sessionId
    });

    return res.FirstOrDefault();
  }

  public string UpsertSession(string userId)
  {
    var sessionId = this.GetSessionByUserId(userId);

    if (sessionId != null)
    {
      return sessionId;
    }

    sessionId = Guid.NewGuid().ToString();

    _dbService.Execute(@"
      INSERT INTO sessions (userId, sessionId)
      VALUES (@UserId, @SessionId)
    ", new
    {
      UserId = userId,
      SessionId = sessionId
    });

    return sessionId;
  }

  public bool ValidateSession(string sessionId)
  {
    var session = GetSessionBySessionId(sessionId);
    return session != null;
  }

  public UserInfo? GetUserBySessionId(string sessionId)
  {
    var user = _dbService.Query<UserInfo>(@"
      SELECT users.userId, name, steamId
      FROM users
        JOIN sessions
        ON users.userId = sessions.userId
      WHERE sessionId = @SessionId
    ", new
    {
      SessionId = sessionId
    }).FirstOrDefault();

    return user;
  }

  public void UpdateUser(UserInfo userInfo)
  {
    _dbService.Execute(@"
      UPDATE users
      SET name = @Name, steamId = @SteamId
      WHERE userId = @UserId
    ", new
    {
      userInfo.Name,
      userInfo.SteamId,
      userInfo.UserId
    });
  }

}