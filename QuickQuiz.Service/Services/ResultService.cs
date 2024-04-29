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

        public async Task AddAsync(AppUser user, float score, Test test, string rating)
        {
            ExamResult result = await _resultRepository.GetResultByUserIdAndTestId(user.Id, test.Id);
            if (result != null)
            {
                result.EndTime = DateTime.UtcNow;
                result.Result = score;
                if (byte.TryParse(rating, out byte rate))
                    result.ExamRating = rate;
                _resultRepository.Update(result);
            }
            else
                await _resultRepository.AddAsync(new ExamResult { Exam = test, Student = user, Result = score });
        }
        public async Task AddVisitorAsync(string visitorName, float score, Test test, string rating)
        {
            ExamResult result = new() { Exam = test, VisitorName = visitorName, Result = score };
            if (byte.TryParse(rating, out byte rate))
                result.ExamRating = rate;
            await _resultRepository.AddAsync(result);
        }
        public async Task<List<ExamResult>> GetExamAllResultAsync(int id)
        {
            return await _resultRepository.GetAllResultByTestIdAsync(id);
        }

        public async Task<List<ExamResult>> GetUserAllResultAsync(AppUser user)
        {
            return await _resultRepository.GetAllResultUserAsync(user);
        }

        public async Task SetStartTimeAsync(AppUser user, Test test)
        {
            await _resultRepository.AddAsync(new ExamResult { Exam = test, StartTime = DateTime.UtcNow, Student = user });
        }
    }
}
