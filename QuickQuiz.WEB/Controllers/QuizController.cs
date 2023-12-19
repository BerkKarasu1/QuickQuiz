using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using System.Web;

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
        //public Task TestGateway(int testId, string visitor) =>
        //    visitor != null ? Test(testId, visitor) : Test(testId);
        [Route("Quiz/Test/{id?}")]

        public async Task<IActionResult> Test(int testId, string visitor="")
        {
            var test = await _testService.GetTestById(testId);
            if (test != null)
                return View((test, visitor));
            return View();
        }
        //public async Task<IActionResult> Test(int testId, string visitor)
        //{
        //    var test = await _testService.GetTestById(testId);
        //    if (test != null)
        //        return View((test, visitor));
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Result(TestDTO testDTO, string visitorName)
        {
            bool result = false;
            if (CurrentUser != null)
                result = await _testService.Result(testDTO, CurrentUser, string.Empty);
            else
                result = await _testService.Result(testDTO, null, visitorName);
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
        [Route("quiz/visitor/{token?}")]
        public async Task<IActionResult> Visitor(string token)
        {
            (string[]? info, bool result) = await _testService.TestLinkControl(token);
            if (result)
                return View((token, string.Empty));
            TempData["ErrorPageMessage"] = "Girmiş olduğunuz link hatalıdır.";
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> VisitorCheck(string token, string name)
        {
            (string[]? info, bool result) = await _testService.TestLinkControl(token);
            if (result)
                return RedirectToAction("Test", "Quiz", new { testId = int.Parse(info[1]), visitor = name }); ;
            TempData["ErrorPageMessage"] = "Girmiş olduğunuz link hatalıdır.";
            return RedirectToAction("Error", "Home");
        }
    }
}
