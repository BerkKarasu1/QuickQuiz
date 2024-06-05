using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Service.Extensions;

namespace QuickQuiz.WEB.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class StatisticController : BaseController
    {
        IStatisticService _statisticService;
        public StatisticController(UserManager<AppUser> userManager, IStatisticService statisticService) : base(userManager)
        {
            _statisticService = statisticService;
        }

        public async Task<IActionResult> Index()
        {
            (QuestionDTO? correctQuestion, QuestionDTO? inCorrectQuestion) = await _statisticService.GetMostCorrectlyAnsweredAndInCorrectlyQuestion(CurrentUser!);
            (Dictionary<TestCategorys, int> testCountByCategory, int totalTestCount) = await _statisticService.GetTestCountByCategory(CurrentUser!);
            (TestDTO mostLikedTest,float mostLikedTestScore) = await _statisticService.GetMostPopularTest(CurrentUser);
            var result = (correctQuestion, inCorrectQuestion, testCountByCategory, totalTestCount,  mostLikedTest,  mostLikedTestScore);
            return View(result);
        }
    }
}
