using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;

namespace QuickQuiz.Core.Repositories
{
    public interface IResultRepository
    {
        public Task AddAsync(ExamResult examResult);
        public Task<List<ExamResult>> GetAllResultUserAsync(AppUser user);
        public Task<List<ExamResult>> GetAllResultByTestIdAsync(int testId);
    }
}
