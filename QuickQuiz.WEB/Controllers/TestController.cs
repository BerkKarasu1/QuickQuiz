using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;

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
        public async Task<IActionResult> CreateTest(List<QuestionDTO> questionDTOs)
        {
            TestDTO testDTO = new()
            {
                Creater = CurrentUser,
                Name = questionDTOs[0].TestName,
                Question = questionDTOs
            };
            await _testService.AddAsync(testDTO);
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
            if (test.PictureFile != null && test.PictureFile.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(test.PictureFile.FileName)}";
                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "TestPictures").PhysicalPath!, randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await test.PictureFile.CopyToAsync(stream);
                test.PictureUrl = randomFileName;
            }
            await _testService.Update(test);
            return RedirectToAction("Index");
        }
    }
}