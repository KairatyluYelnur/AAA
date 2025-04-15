// Models/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace AAA.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Имя пользователя должно быть не менее 3 символов.", MinimumLength = 3)]
        public required string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен содержать не менее 6 символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
