using Microsoft.AspNetCore.Http;
using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Dtos
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUser Creater { get; set; }
        public List<QuestionDTO>? Question { get; set; } = new List<QuestionDTO>();
        public IFormFile? PictureFile { get; set; }
        public string PictureUrl { get; set; }  
    }
}
