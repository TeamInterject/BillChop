using System.ComponentModel.DataAnnotations;
using BillChopBE.DataAccessLayer.Models.Validation;

namespace BillChopBE.Services.Configurations
{
  public class JwtConfig : ValidatableModel
  {
    [Required]
    public string Key { get; set; } = null!;

    [Required]
    public string Issuer { get; set; } = null!;

    [Required]
    public string Audience { get; set; } = null!;

    [Required]
    public string Subject { get; set; } = null!;
  }
}