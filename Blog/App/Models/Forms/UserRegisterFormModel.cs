using System.ComponentModel.DataAnnotations;

namespace Blog.App.Models.Forms;

public class UserRegisterFormModel
{
    [Required(ErrorMessage = "Der volle Name muss angegeben werden.")]
    public string FullName { get; set; } = "";
    
    [Required(ErrorMessage = "Es muss ein Benutzername angegeben werden.")]
    public string Username { get; set; } = "";
    
    [Required(ErrorMessage = "Es muss eine Email angegeben werden."), EmailAddress]
    public string Email { get; set; } = "";
    
    [Required(ErrorMessage = "Es muss ein Passwort angegeben werden."), MinLength(8, ErrorMessage = "Das Passwort muss mindestens acht Zeichen lang sein.")]
    public string Password { get; set; } = "";
}