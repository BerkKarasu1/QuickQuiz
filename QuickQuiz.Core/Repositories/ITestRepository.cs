using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Repositories
{
    public interface ITestRepository:IGenericRepository<Test>
    {
        public Task<List<Test>> GetAllTest(AppUser user);
        public Task<Test> GetTestById(int id);
    }
}
