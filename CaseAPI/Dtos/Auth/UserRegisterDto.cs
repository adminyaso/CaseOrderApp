using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Auth
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı 50 karakteri aşmamalıdır.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }
    }
}
