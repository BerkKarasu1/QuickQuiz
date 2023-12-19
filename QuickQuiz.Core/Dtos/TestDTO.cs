using Microsoft.AspNetCore.Http;
using QuickQuiz.Core.Model;
using System.ComponentModel.DataAnnotations;

namespace QuickQuiz.Core.Dtos
{
    public record TestDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Test adı boş bırakılamaz!")]
        public string Name { get; set; }
        public UserEditViewModel? Creater { get; set; }
        public List<QuestionDTO>? Question { get; set; }
        public string Link { get; set; }
        public IFormFile? PictureFile { get; set; }
        [Required(ErrorMessage = "Kategori boş bırakılamaz!")]
        public TestCategorys TestCategorys { get; set; }
        public string TestCategoryDescription { get; set; }
        public string? PictureUrl { get; set; }
    }
}
