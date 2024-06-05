using Microsoft.EntityFrameworkCore;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Repository.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        AppDbContext _context;
        public QuestionRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task AddAsync(Question entity)
        {
            try
            {
                await _context.Questions.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var aasdasd = e.Data;
                throw;
            }
        }

        public async Task<Question> FindQuestionByIdAsync(int id)
        {
            return await _context.Questions.Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Question>> GetAllQuestion(AppUser user)
        {
            return await _context.Questions.Where(x => x.Creater.Id == user.Id).Include(x=>x.Answers).ToListAsync();
        }
        public async Task<List<Question>> GetAllQuestionsWithStatistic(AppUser user)
        {
            return await _context.Questions.Where(x => x.Creater.Id == user.Id).Include(x=>x.Answers).Include(x=>x.Statistic).ToListAsync();
        }

        public void Remove(Question entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Question entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
