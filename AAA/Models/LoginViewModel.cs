// Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace AAA.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно для заполнения")]
        [StringLength(50, MinimumLength = 3,
           ErrorMessage = "Имя пользователя должно быть от 3 до 50 символов")]
        [Display(Name = "Имя пользователя")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Пароль должен быть от 3 до 100 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public required string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
