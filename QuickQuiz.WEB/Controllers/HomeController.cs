using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using QuickQuiz.WEB.Extensions;
using QuickQuiz.WEB.Models;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;

namespace QuickQuiz.WEB.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        readonly ITestService _testService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IEmailService emailService, ITestService testService) : base(userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _testService = testService;
        }
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index()
        {
            List<TestDTO> test = await _testService.GetAllTestAsync();
            UserEditViewModel user = CurrentUser.Adapt<UserEditViewModel>();
            List<List<TestDTO>> categories = test.GroupBy(x => x.TestCategorys).Select(g => g.Take(10).ToList()).ToList();

            return View((test, user, categories));
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            var tuple = (new SignInViewModel(), new SignUpViewModel());
            return View(tuple);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn([Bind(Prefix = "Item1")] SignInViewModel model, string? returnUrl = null)
        {
            var tuple = (model, new SignUpViewModel());
            if (!ModelState.IsValid)
                return View(tuple);

            var hasUser = await _userManager.FindByNameAsync(model.UserName);
            hasUser ??= await _userManager.FindByEmailAsync(model.UserName);

            if (hasUser == null)
                return ReturnSingInError(tuple, "Giriş bilgileriniz hatalıdır. Kontrol edip tekrar giriş yapınız.");
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, false);

            if (signInResult.Succeeded)
            {
                returnUrl ??= Url.Action("Index", "Home");
                return Redirect(returnUrl!);
            }
            else if (signInResult.IsLockedOut)
                return ReturnSingInError(tuple, "3 dk boyunca giriş yapamazsınız!");
            else
                return ReturnSingInError(tuple, "Giriş bilgileriniz hatalıdır. Kontrol edip tekrar giriş yapınız.");
        }
        private ViewResult ReturnSingInError((SignInViewModel, SignUpViewModel) tuple, string errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return View(tuple);
        }
        public IActionResult SignUp()
        {
            return RedirectToAction(nameof(HomeController.SignIn));
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([Bind(Prefix = "Item2")] SignUpViewModel request)
        {
            if (!ModelState.IsValid)
                return View();
            AppUser user = new()
            {
                UserName = request.UserName,
                Email = request.Email
            };
            var identityResult = await _userManager.CreateAsync(user, request.PasswordConfirm);
            if (identityResult.Succeeded)
            {
                await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                string? callbackUrl = Url.Action("ConfirmEmail", "Home", new { token = encodedToken, userName = request.UserName, email = request.Email }, Request.Scheme);
                if (callbackUrl is null)
                {
                    //LOG İŞLEMİ YAPILACAK.
                }
                string encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
                await _emailService.SendAccountConfirmEmail(encodedUrl, request.UserName, request.Email);
                //todo: Mesaj yönlendirmesi düzeltilecek
                TempData["SuccessMessage"] = "Üyelik kayıt işleminizin tamamlanması için Mail adresinize gönderilen aktivasyon linki üzerinden üyeliğinizi onaylayın.";
            }
            else
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
            return RedirectToAction(nameof(HomeController.SignIn));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string token, string userName, string email)
        {
            string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    //todo: Başarı mesajı + Login Ekranı
                }
                else
                {
                    //todo: Başarısız mesajı + Tekrar Token Gönderme Ekranı
                }
            }
            return RedirectToAction(nameof(HomeController.SignIn));
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
            string passwordResetLink = Url.Action("PasswordChange", "Member",
                new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            //örnek link : https://localhost:7295?userId?12213&token=dshgdfhsadsd  bu sekilde bir url üretilecek.
            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki, e-posta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}