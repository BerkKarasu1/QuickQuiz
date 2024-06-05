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
    public class TestRepository : ITestRepository
    {
        AppDbContext _context;
        public TestRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task AddAsync(Test entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        //todo: Take(2) alanı kaldırılacak.
        public async Task<List<Test>> GetAllTest(AppUser user)
        {
            return await _context.Tests.Where(x => x.Creater.Id == user.Id).OrderByDescending(x => x.Id).ToListAsync();
        }
        public async Task<List<Test>> GetAllTest()
        {
            return await _context.Tests.Include(x=>x.Creater).OrderByDescending(x=>x.Id).ToListAsync();
        }

        public async Task<Test> GetTestById(int id)
        {
            return await _context.Tests.Include(x => x.Creater).Include(x => x.Question).ThenInclude(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Remove(Test entity)
        {
            _context.Tests.Remove(entity);
            _context.SaveChanges();
        }
        public void Update(Test entity)
        {
            _context.Tests.Update(entity);
            _context.SaveChanges();
        }
    }
}
