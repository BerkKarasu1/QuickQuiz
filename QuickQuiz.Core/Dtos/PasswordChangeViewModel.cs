using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Eski Şifre:")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir!")]
        public string PasswordOld { get; set; } = null!;  // nullable 

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifre alanı boş bırakılamaz!")]
        [Display(Name = "Yeni Şifre:")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir!")]
        public string PasswordNew { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Girmiş olduğunuz şifreler aynı değildir!")]
        [Required(ErrorMessage = "Yeni şifre tekrar alanı boş bırakılamaz!")]
        [Display(Name = "Yeni Şifre Tekrar:")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir!")]
        public string PasswordNewConfirm { get; set; } = null!;
        public string? Token { get; set; }
    }
}
