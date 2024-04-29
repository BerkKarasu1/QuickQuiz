using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using System.ComponentModel;
using System.Reflection;

namespace QuickQuiz.WEB.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class QuestionController : BaseController
    {
        readonly IQuestionService _questionService;
        public QuestionController(UserManager<AppUser> userManager, IQuestionService questionService) : base(userManager)
        {
            _questionService = questionService;
        }

        public IActionResult Add() => View(new QuestionDTO());
        
        [HttpPost]
        public async Task<IActionResult> Add(QuestionDTO questionDTO)
        {
            await _questionService.AddAsync(questionDTO,CurrentUser);
            return RedirectToAction("Add");
        }
        [HttpPost]
        public async Task<IActionResult> BulkAddWithExcel(QuestionDTO questionDTO)
        {
            await _questionService.AddAsync(questionDTO, CurrentUser);
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
            List<string> enums = new();
            foreach (TestCategorys testCategory in Enum.GetValues(typeof(TestCategorys)))
            {
                var desc = testCategory.GetType().GetField(testCategory.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description;
                if (desc != null)
                    enums.Add(desc);
            }
            ViewBag.genderList = new SelectList(enums);
            List<QuestionDTO> allQuestions = await _questionService.GetAllQuestionAsync(CurrentUser);
            return View((allQuestions, new TestDTO()));
        }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
