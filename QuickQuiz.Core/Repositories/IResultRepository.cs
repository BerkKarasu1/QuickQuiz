using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;
using System.Security.Cryptography;

namespace QuickQuiz.Core.Repositories
{
    public interface IResultRepository
    {
        public Task AddAsync(ExamResult examResult);
        void Update(ExamResult examResult);
        public Task<List<ExamResult>> GetAllResultUserAsync(AppUser user);
        public Task<List<ExamResult>> GetAllResultByTestIdAsync(int testId);
        public Task<ExamResult> GetResultByUserIdAndTestId(string userId, int testId);
    }
}
