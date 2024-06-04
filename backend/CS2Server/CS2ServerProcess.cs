

using System.Diagnostics;

public class CS2ServerProcess : IDisposable
{

  private string executablePath;

  private Process process = new Process();

  private Guid id = System.Guid.NewGuid();


  public CS2ServerProcess(
    string executablePath
  )
  {
    this.executablePath = executablePath;
  }

  public void Dispose()
  {
    this.process.Kill();
    this.process.WaitForExit();
    this.process.Dispose();
  }

  public void Start()
  {
    this.process.StartInfo.FileName = this.executablePath;
    this.process.StartInfo.CreateNoWindow = true;
    this.process.StartInfo.ArgumentList.Add("-dedicated");
    this.process.StartInfo.ArgumentList.Add("-map de_dust2");

    this.process.Start();
  }

  public void Kill()
  {
    this.Dispose();
  }


  public void SetMap(String mapName)
  {
    this.process.StandardInput.WriteLine(mapName);
    this.process.StandardInput.Flush();
  }




}