using Microsoft.AspNetCore.Http;

namespace QuickQuiz.Core.Dtos
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserEditViewModel? Creater { get; set; }
        public List<QuestionDTO>? Question { get; set; } = new List<QuestionDTO>();
        public IFormFile? PictureFile { get; set; }
        public string PictureUrl { get; set; }  
    }
}
