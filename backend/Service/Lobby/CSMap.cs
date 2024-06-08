namespace giga_inhouse.maps;
public record CSMap
{
  public required string Name { get; init; }
  public required string Title { get; init; }
  public required string Image { get; init; }
}



public static class MapPool
{
  public static IEnumerable<CSMap> Maps { get; } = new List<CSMap>{
    new CSMap{Name = "de_dust2", Title = "Dust2", Image = ""},
    new CSMap{Name = "de_vertige", Title = "Vertigo ", Image = ""}
  };
}