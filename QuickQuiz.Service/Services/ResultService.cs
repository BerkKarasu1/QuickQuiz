using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Core.Services;

namespace QuickQuiz.Service.Services
{
    public class ResultService : IResultService
    {
        IResultRepository _resultRepository;
        public ResultService(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }
        public async Task AddAsync(ExamResult result)
        {
            await _resultRepository.AddAsync(result);
        }

        public async Task AddAsync(AppUser user, float score, Test test)
        {
            await _resultRepository.AddAsync(new ExamResult { Exam = test, Student = user, Result = score });
        }

        public async Task<List<ExamResult>> GetExamAllResultAsync(int id)
        {
            return await _resultRepository.GetAllResultByTestIdAsync(id);
        }

        public async Task<List<ExamResult>> GetUserAllResultAsync(AppUser user)
        {
            return await _resultRepository.GetAllResultUserAsync(user);
        }
    }
}
