using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public  record SignInViewModel
    {
        public SignInViewModel()
        {

        }
        public SignInViewModel(string password, string name)
        {
            UserName = name;
            Password = password;
        }

        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı:")]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Şifre:")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir!")]
        public string Password { get; set; } = null!;

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
