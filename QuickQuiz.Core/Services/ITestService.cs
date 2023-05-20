using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Services
{
    public interface ITestService
    {
        Task AddAsync(TestDTO testDTO);
        void Remove(TestDTO TestDTO);
        Task<List<TestDTO>> GetAllTest(AppUser user);
        Task<TestDTO> TestControl(AppUser user, int id);
        Task Update(TestDTO TestDTO);
    }
}
