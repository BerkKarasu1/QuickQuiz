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
            await _context.Questions.AddAsync(entity);
        }

        public async Task<Question> FindQuestionByIdAsync(int id)
        {
          return  await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Question>> GetAllQuestion(AppUser user)
        {
            return await _context.Questions.Where(x => x.Creater == user).ToListAsync();
        }

        public void RemoveAsync(Question entity)
        {
            _context.Remove(entity);
        }

        public void UpdateAsync(Question entity)
        {
            _context.Update(entity);
        }
    }
}
