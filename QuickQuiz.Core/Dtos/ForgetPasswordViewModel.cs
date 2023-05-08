using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış!")]
        [Display(Name = "Email:")]
        public string Email { get; set; } = null!;
    }
}
