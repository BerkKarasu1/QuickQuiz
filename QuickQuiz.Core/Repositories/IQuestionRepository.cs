using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Repositories
{
    public interface IQuestionRepository:IGenericRepository<Question>
    {
       public Task<Question> FindQuestionByIdAsync(int id);
       public Task<List<Question>> GetAllQuestion(AppUser user);
        Task<List<Question>> GetAllQuestionsWithStatistic(AppUser user);
    }
}
