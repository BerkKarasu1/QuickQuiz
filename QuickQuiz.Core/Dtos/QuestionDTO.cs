using Mapster;
using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Dtos
{
    public record class QuestionDTO
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<AnswerDTO> Answers { get; set; }
        public AnswerDTO? TrueAnswer { get; set; } 
        public List<TestDTO> Tests { get; set; }
        public UserDTO Creater { get; set; }
        public bool Check { get; set; } = false;
        public string? TestName { get; set; }
    }
    public record class AnswerDTO
    {
        public int Id { get; set; }
        //public QuestionDTO Question { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; } 
    }
}
