using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Org.BouncyCastle.Crypto.IO;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using System.Drawing;
using System.IO;
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
        public async Task<IActionResult> Test(int id, string visitor = "")
        {
            var test = await _testService.GetTestById(id);
            if (test != null)
            {
                if (CurrentUser != null)
                {
                    await _testService.SetStartTimeAsync(CurrentUser, test);
                    visitor = "";
                }
                return View((test, visitor, ""));
            }
            return RedirectToAction("Index", "Home");
        }
        //public async Task<IActionResult> Test(int testId, string visitor)
        //{
        //    var test = await _testService.GetTestById(testId);
        //    if (test != null)
        //        return View((test, visitor));
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Result(TestDTO testDTO, string visitorName, string rating = "")
        {
            bool result = await _testService.Result(testDTO, CurrentUser, visitorName, rating);
            if (result)
            {
                var quizResults = await _resultService.GetExamAllResultAsync(testDTO.Id);
                return View((quizResults, CurrentUser != null ? CurrentUser.UserName : visitorName));
            }
            return View();
        }
        public async Task<IActionResult> Result(int id)
        {
            var quizResults = await _resultService.GetExamAllResultAsync(id);
            if (quizResults != null)
                return View((quizResults, CurrentUser?.UserName));
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
                return RedirectToAction("Test", "Quiz", new { id = int.Parse(info[1]), visitor = name }); ;
            TempData["ErrorPageMessage"] = "Girmiş olduğunuz link hatalıdır.";
            return RedirectToAction("Error", "Home");
        }
        public async Task<IActionResult> ExcelPage(int id)
        {
            var quiz = await _testService.GetTestById(id);
            if (quiz != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var package = new ExcelPackage();
                package.Workbook.Worksheets.Add(quiz.Name);
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                ws.Name = quiz.Name;
                int questCounter = 0;
                ws.Cells[1, 2].Value = "Soru";
                for (int i = 2; i < 8; i++)
                {
                    ws.Column(i).Width = 60;
                    ws.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[1, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[1, i].Style.Font.Bold = true;
                    ws.Cells[1, i].Style.Font.UnderLine = true;
                    if (i > 2)
                        ws.Cells[1, i].Value = $"Cevap {(char)(i + 62)}";
                }
                foreach (var item in quiz.Question)
                {
                    questCounter++;
                    ws.Cells[questCounter + 1, 1].Value = $"{questCounter}. Soru ";
                    ws.Cells[questCounter + 1, 1].Style.Font.Bold = true;
                    ws.Cells[questCounter + 1, 2].Value = item.Question;
                    ws.Cells[questCounter + 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[questCounter + 1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    for (int i = 1; i <= item.Answers.Count; i++)
                    {
                        ws.Cells[questCounter + 1, i + 2].Value = item.Answers[i - 1].AnswerText;
                        ws.Cells[questCounter + 1, i + 2].Style.WrapText = true;
                        ws.Cells[questCounter + 1, i + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[questCounter + 1, i + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        if (item.Answers[i - 1].IsCorrect)
                            ws.Cells[questCounter + 1, i + 2].Style.Font.Color.SetColor(Color.Green);
                    }
                }
                return File(package.GetAsByteArray().ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{quiz.Name} by QuizCk.xlsx");
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> PdfPage(int id)
        {
            var quiz = await _testService.GetTestById(id);
            if (quiz != null)
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                MemoryStream memStream = new();
                Document pdfDoc = new();
                PdfWriter pdfWriter;
                pdfDoc.SetPageSize(PageSize.A4);
                pdfWriter = PdfWriter.GetInstance(pdfDoc, memStream);
                pdfDoc.Open();
                //PDFBackgroundHelper pageEventHelper = new PDFBackgroundHelper();
                //pdfWriter.PageEvent = pageEventHelper;
                BaseFont STF_Helvetica_Turkish = BaseFont.CreateFont("Helvetica", "CP1254", BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fontTitle = new(STF_Helvetica_Turkish, 12, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font fontNormal = new(STF_Helvetica_Turkish, 12, iTextSharp.text.Font.NORMAL);
                Paragraph p = new()
                {
                    Alignment = Element.ALIGN_CENTER
                };
                Chunk blankChunk = new("\n");
                Chunk doubleBlankChunk = new("\n\n");
                Phrase pBlank = new(blankChunk); ;
                p.Alignment = Element.ALIGN_CENTER;
                p.Add(pBlank);
                Chunk title = new(quiz.Name, fontTitle);
                title.Font.SetStyle("bold underline");
                p.Add(title);
                p.Add(blankChunk);
                p.Add(doubleBlankChunk);
                int questCounter = 0;
                List<char> trueAnswer = new();
                foreach (var item in quiz.Question)
                {
                    Paragraph paragraph = new();
                    questCounter++;
                    Chunk question = new Chunk(questCounter + ". " + item.Question, fontNormal);
                    paragraph.Add(question);
                    paragraph.Add(blankChunk);
                    int pageNumb = pdfWriter.PageNumber;
                    for (int i = 1; i <= item.Answers.Count; i++)
                    {
                        Chunk answer = new((char)(i + 64) + ") " + item.Answers[i - 1].AnswerText, fontNormal);
                        if (item.Answers[i - 1].IsCorrect)
                            trueAnswer.Add((char)(i + 64));
                        paragraph.Add(answer);
                        paragraph.Add(blankChunk);
                    }
                    if (pdfWriter.PageNumber != pageNumb)
                        pdfDoc.NewPage();
                    p.Add(paragraph);
                    p.Add(doubleBlankChunk);
                }
                pdfDoc.Add(p);
                pdfDoc.NewPage();
                PdfPTable table = new(2);
                //table.SetWidths(new[] { 1f, 1f });
                PdfPCell cell = new(new Phrase("Cevaplar"));
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                for (int i = 0; i < trueAnswer.Count; i++)
                {
                    table.AddCell($"{i + 1}");
                    table.AddCell(trueAnswer[i].ToString());
                }
                pdfDoc.Add(table);
                pdfDoc.Dispose();
                try
                {
                    pdfDoc.Close();
                    pdfWriter.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return File(memStream.ToArray(), "application/pdf", $"{quiz.Name} by QuizCk.pdf");
            }
            return null;
        }
    }
}
