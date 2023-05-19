using Microsoft.AspNetCore.Http;
using QuickQuiz.Core.Model;
using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Ad alanı boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı:")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış!")]
        [Display(Name = "Email:")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz!")]
        [Display(Name = "Telefon:")]
        public string Phone { get; set; } = null!;  // nullable olamaz

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi:")]
        public DateTime? BirthDate { get; set; } // nullable olabilir

        [Display(Name = "Şehir:")]
        public string? City { get; set; }

        [Display(Name = "Profil Resmi:")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Cinsiyet:")]
        public Gender? Gender { get; set; }

        [Display(Name = "Twitter Link:")]
        public string? Twitter { get; set; }

        [Display(Name = "Facebook Link:")]
        public string? Facebook { get; set; }

        [Display(Name = "Instagram Link:")]
        public string? Instagram { get; set; }

        [Display(Name = "Linkedln Link:")]
        public string? Linkedln { get; set; }

        [Display(Name = "Github Link:")]
        public string? Github { get; set; }
    }
}
