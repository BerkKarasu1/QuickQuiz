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
        public IActionResult Add(QuestionDTO questionDTO)
        {//todo: burada null olan answerlar silinecek.
            //true answer Id'si ile geçerli cevapId'si birleştirilecek. Türü değiştirilip answer yapılabilir.
            questionDTO.Creater = CurrentUser;
            _questionService.AddAsync(questionDTO);            
            return View();
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
    }
}
