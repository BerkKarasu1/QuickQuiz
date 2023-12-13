using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Services
{
    public interface IQuestionService
    {
       Task<QuestionDTO> GetQuestionByIdAsync(int questionId);
        Task<List<QuestionDTO>> GetAllQuestionAsync(AppUser user);
        void Update(QuestionDTO question);
        void Remove(QuestionDTO question);
        Task AddAsync(QuestionDTO question, AppUser user);
    }
}
