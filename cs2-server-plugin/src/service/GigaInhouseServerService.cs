

using System.Net.Http.Headers;
using System.Net.Http.Json;

public class GigaInhouseServerService
{

  static readonly HttpClient _client;

  static GigaInhouseServerService()
  {
    _client = new HttpClient
    {
      BaseAddress = new Uri("http://192.168.1.123:5104/"),
    };
  }

  public async Task<Lobby> GetSessionLobby()
  {
    using (var request = new HttpRequestMessage(HttpMethod.Get, "http://192.168.1.123:5104/Lobby"))
    {
      Console.WriteLine(request.RequestUri);
      request.Headers.Add("Session-Token", "giga-admin");
      var res = await _client.SendAsync(request);
      var lobby = await res.Content.ReadFromJsonAsync<Lobby>();

      if (lobby == null)
      {
        throw new Exception("No lobby");
      }

      return lobby;

    }


  }
}