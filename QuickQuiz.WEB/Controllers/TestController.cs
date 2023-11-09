using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using File = System.IO.File;

namespace QuickQuiz.WEB.Controllers
{
    public class TestController : BaseController
    {
        private ITestService _testService;
        private readonly IFileProvider _fileProvider;
        public TestController(UserManager<AppUser> userManager, ITestService testService, IFileProvider fileProvider) : base(userManager)
        {
            _testService = testService;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        
        {
            var tests = await _testService.GetAllTest(CurrentUser);
            return View(tests);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTest([Bind(Prefix = "Item1")] List<QuestionDTO> questionDTOs, [Bind(Prefix = "Item2")] TestDTO test)
        {
            test.Question = questionDTOs;
            foreach (TestCategorys testCategory in Enum.GetValues(typeof(TestCategorys)))
            {
                var desc = testCategory.GetType().GetField(testCategory.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description;
                if (desc != null && desc == test.TestCategoryDescription.ToString())
                {
                    test.TestCategorys = testCategory;
                    break;
                }
            }
            await _testService.AddAsync(test, CurrentUser);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveTest(TestDTO testDTO)
        {
            _testService.Remove(testDTO);
            return RedirectToAction("Index");
        }
        [Route("Test/Edit/{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            var test = await _testService.TestControl(CurrentUser, id);
            if (test != null)
                return View(test);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edits(TestDTO test)
        {
            await _testService.Update(test);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Test/External/{link?}")]
        public async Task<IActionResult> ExternalUserTest(string link)        
        {
            (bool result, int testId) = await _testService.TestLinkControl(link);
            testId = 9;
            return View((result, testId,""));
        }
        [HttpPost]
        public async Task<IActionResult> ExternalUserTestPost(string name, int id)
        {
            //Ad boş olamaz şeklinde uyarı gidecek
            if (string.IsNullOrEmpty(name))
                return View((true, id));
            return RedirectToAction("Test", "Quiz", id);
        }
    }
}