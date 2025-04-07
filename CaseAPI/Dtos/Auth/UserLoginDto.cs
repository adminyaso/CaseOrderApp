using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Auth
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; }
    }
}
