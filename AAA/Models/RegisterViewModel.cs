// Models/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace AAA.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 50 символов")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Полное имя обязательно")]
        [Display(Name = "Полное имя")]
        public required string FullName { get; set; }

        [Display(Name = "Создать как администратора")]
        public bool IsAdmin { get; set; }
    }
}
