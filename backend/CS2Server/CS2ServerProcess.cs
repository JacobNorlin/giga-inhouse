

using System.Diagnostics;

public class CS2ServerProcess : IDisposable
{

  private string _executablePath;

  private Process _process = new Process();


  public CS2ServerProcess(
    string executablePath
  )
  {
    _executablePath = executablePath;
  }

  public void Dispose()
  {
    _process.Kill();
    _process.WaitForExit();
    _process.Dispose();
  }

  public void Start()
  {
    _process.StartInfo.FileName = _executablePath;
    _process.StartInfo.CreateNoWindow = true;
    _process.StartInfo.ArgumentList.Add("-dedicated");
    _process.StartInfo.ArgumentList.Add("-map de_dust2");

    _process.Start();
  }

  public void Kill()
  {
    Dispose();
  }


  public void SetMap(String mapName)
  {
    _process.StandardInput.WriteLine(mapName);
    _process.StandardInput.Flush();
  }




}