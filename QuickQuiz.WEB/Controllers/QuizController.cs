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
        IResultService _resultService;
        public QuizController(UserManager<AppUser> userManager, ITestService testService, IResultService resultService) : base(userManager)
        {
            _testService = testService;
            _resultService = resultService;
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
            bool result = await _testService.Result(testDTO, CurrentUser);
            if (result)
            {
                var quizResults = await _resultService.GetExamAllResultAsync(testDTO.Id);
                return View(quizResults);
            }
            return View();
        }
        public async Task<IActionResult> Result(int id)
        {
            var quizResults = await _resultService.GetExamAllResultAsync(id);
            if (quizResults != null)
                return View(quizResults);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            var results = await _resultService.GetUserAllResultAsync(CurrentUser);
            return View(results);
        }
    }
}
