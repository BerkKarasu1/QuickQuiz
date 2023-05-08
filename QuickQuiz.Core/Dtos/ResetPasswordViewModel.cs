using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Yeni Şifre:")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Girmiş olduğunuz şifreler aynı değildir!")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Display(Name = "Yeni Şifre Tekrar:")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
