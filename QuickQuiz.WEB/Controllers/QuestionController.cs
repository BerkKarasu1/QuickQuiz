using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;

namespace QuickQuiz.WEB.Controllers
{
    public class QuestionController : BaseController
    {
        readonly IQuestionService _questionService;
        public QuestionController(UserManager<AppUser> userManager, IQuestionService questionService) : base(userManager)
        {
            _questionService = questionService;
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(QuestionDTO questionDTO)
        {
            questionDTO.Creater = CurrentUser;
            await _questionService.AddAsync(questionDTO);
            return RedirectToAction("Add");
        }
        [HttpPost]
        public IActionResult Delete(QuestionDTO questionDTO)
        {
            _questionService.Remove(questionDTO);
            return View();
        }
        public IActionResult Update(QuestionDTO questionDTO)
        {
            _questionService.Update(questionDTO);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllQuestions(List<QuestionDTO> questionDTOs)
        {
            List<QuestionDTO> allQuestions = await _questionService.GetAllQuestionAsync(CurrentUser);
            return View(allQuestions);
        }
    }
}
