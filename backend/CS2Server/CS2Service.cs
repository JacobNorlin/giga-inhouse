

public class CS2Service
{


  private string executablePath;

  public CS2Service(IConfiguration configuration)
  {
    this.executablePath = configuration["CS2Settings:ExecutablePath"]!;
  }

  public void StartGame()
  {
    var process = new CS2ServerProcess(this.executablePath);

    process.Start();


  }

}