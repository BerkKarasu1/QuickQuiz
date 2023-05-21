using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;

namespace QuickQuiz.WEB.Controllers
{
    public class QuizController : BaseController
    {
        ITestService _testService;
        public QuizController(UserManager<AppUser> userManager, ITestService testService) : base(userManager)
        {
            _testService = testService;
        }
        [Route("Quiz/Test/{id?}")]
        public async Task<IActionResult> Test(int id)
        {
            var test = await _testService.GetTestById(id);
            if (test != null)
                return View(test);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Result(TestDTO testDTO)
        {
            bool result = await _testService.Result(testDTO);
            if (result)
                return View();
            return View();
        }

    }
}
