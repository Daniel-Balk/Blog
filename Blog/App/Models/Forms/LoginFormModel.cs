using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models.Forms;

public class LoginFormModel
{
    [Required]
    public string Password { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }
}