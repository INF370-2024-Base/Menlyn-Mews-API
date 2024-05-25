using System.ComponentModel.DataAnnotations;

namespace Menlyn_Mews_API.Models.Domain.Login
{
    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
