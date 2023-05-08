using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {

        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        [Required(ErrorMessage = "Kullanıcı Ad alanı boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı:")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış!")]
        [Display(Name = "Email:")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Şifre:")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir!")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Girmiş olduğunuz şifreler aynı değildir!")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Display(Name = "Şifre Tekrar:")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir!")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
