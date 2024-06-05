using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Repositories
{
    public interface IStatisticService
    {
        Task SetQuestionStatistics(Dictionary<int, bool> answerStatistics);
        Task<(QuestionDTO?, QuestionDTO?)> GetMostCorrectlyAnsweredAndInCorrectlyQuestion(AppUser user);
        Task<(Dictionary<TestCategorys, int>, int)> GetTestCountByCategory(AppUser user);
        Task<(TestDTO?, float)> GetMostPopularTest(AppUser user);
    }
}
