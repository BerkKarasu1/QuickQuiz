using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using QuickQuiz.WEB.Extensions;
using QuickQuiz.WEB.Models;
using System.Diagnostics;

namespace QuickQuiz.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        readonly ITestService _testService;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, ITestService testService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _testService = testService;
        }

        public async Task<IActionResult> Index()
        {
            var test = await _testService.GetAllTestAsync();
            return View(test);
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            var tuple = (new SignInViewModel(), new SignUpViewModel());
            return View(tuple);
        }
        public IActionResult SignMenu()
        {
            var tuple = (new SignInViewModel(), new SignUpViewModel());
            return View(tuple);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn([Bind(Prefix = "Item1")] SignInViewModel model, string returnUrl = null)
        {
             var tuple = (model, new SignUpViewModel());
            if (!ModelState.IsValid)
            {
                return View(tuple);
            }

            returnUrl ??= Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByNameAsync(model.UserName);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış!");
                return View(tuple);
            }
            //lockout =true olarak işaretle
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, false);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl!);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dk boyunca giriş yapamazsınız!" });
                return View(tuple);
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya şifreniz yanlış!", $"Başarısız giriş sayısı: {await _userManager.GetAccessFailedCountAsync(hasUser)}" });

            return View(tuple);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([Bind(Prefix = "Item2")] SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var identityResult = await _userManager.CreateAsync(new AppUser() { UserName = request.UserName, Email = request.Email }, request.PasswordConfirm);
            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşmiştir.";
                return RedirectToAction(nameof(HomeController.SignIn));
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);  // kullanıcı var mı yok mu
            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");
                return View(); // redirect dersek hatayı kaybederiz.
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("PasswordChange", "Member",
                new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            //örnek link : https://localhost:7295?userId?12213&token=dshgdfhsadsd  bu sekilde bir url üretilecek.

            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki, e-posta adresinize gönderilmiştir.";

            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}