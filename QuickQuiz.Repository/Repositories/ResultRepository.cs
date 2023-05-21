using Microsoft.EntityFrameworkCore;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;
using QuickQuiz.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Repository.Repositories
{
    public class ResultRepository : IResultRepository
    {
        AppDbContext _context;
        public ResultRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ExamResult examResult)
        {            
                await _context.ExamResults.AddAsync(examResult);
                await _context.SaveChangesAsync();
        }

        public async Task<List<ExamResult>> GetAllResultByTestIdAsync(int testId)
        {
            return await _context.ExamResults.Where(x => x.Exam.Id == testId).OrderByDescending(x => x.Result).ToListAsync();
        }

        public async Task<List<ExamResult>> GetAllResultUserAsync(AppUser user)
        {
            return await _context.ExamResults.Where(x => x.Student.Id == user.Id).OrderByDescending(x => x.Result).ToListAsync();
        }
    }
}
