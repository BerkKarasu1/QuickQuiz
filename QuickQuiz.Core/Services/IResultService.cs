using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Services
{
    public interface IResultService
    {
        Task AddAsync(ExamResult result);
        Task AddAsync(AppUser user,float score,Test test,string rating);
        Task SetStartTimeAsync(AppUser user, Test test);
        Task AddVisitorAsync(string visitorName, float score, Test test, string rating);
		Task<List<ExamResult>> GetUserAllResultAsync(AppUser user);
        Task<List<ExamResult>> GetExamAllResultAsync(int id);
    }
}
