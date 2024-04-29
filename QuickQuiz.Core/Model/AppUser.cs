using Microsoft.AspNetCore.Identity;
using QuickQuiz.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace QuickQuiz.Core.Model
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public Occupation? Occupation { get; set; }
        public List<Question>? Question { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Linkedln { get; set; }
        public string? Github { get; set; }
        public List<Test>? Tests { get; set; }
        public DateTime? RegisterTime { get; set; }
        public DateTime? AccountConfirmTime { get; set; }
    }
}