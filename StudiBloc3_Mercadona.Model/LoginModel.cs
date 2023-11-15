using System.ComponentModel.DataAnnotations;

namespace StudiBloc3_Mercadona.Model;

public class LoginModel
{
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username peut contenir seulement des lettres et des chiffres.")]
    public string Username { get; set; }

    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password doit contenir au minimum huit caractères, au moins une lettre et un chiffre.")]
    public string Password { get; set; }
}