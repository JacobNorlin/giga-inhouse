namespace backend.Controllers;


public class Error
{
    public Error(string type, string description)
    {
        Type = type;
        Description = description;
    }

    public string Type { get; private set; }
    public string Description { get; private set; }
}