

public class CS2Service
{


  private string _executablePath;

  public CS2Service(IConfiguration configuration)
  {
    _executablePath = configuration["CS2Settings:ExecutablePath"]!;
  }

  public void StartGame()
  {
    var process = new CS2ServerProcess(_executablePath);

    process.Start();


  }

}