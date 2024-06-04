

using System.ComponentModel.DataAnnotations;

public class LoginUser
{
  [Required]
  public string? UserId { get; set; }

  [Required]
  public string? ProvidedPassword { get; set; }

}