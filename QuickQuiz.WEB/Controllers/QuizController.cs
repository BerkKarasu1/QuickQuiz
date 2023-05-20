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
        [Route("Quiz/TryTest/{id?}")]
        public async Task<IActionResult> TryTest(int id)
        {
           var test= await _testService.TestControl(CurrentUser, id);
            if(test!=null)
                return View(test);
            return View();
        }
        [HttpPost]
        public  IActionResult Result(TestDTO testDTO)
        {
            return View();
        }
    }
}
