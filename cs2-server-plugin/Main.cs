using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace cs2_server_plugin;

public class Main : BasePlugin
{

  public override string ModuleName => "Giga inhouse";
  public override string ModuleVersion => "0.0.1";
  public override void Load(bool hotReload)
  {
    Console.WriteLine("Loaded giga-inhouse plugin");
  }

}
