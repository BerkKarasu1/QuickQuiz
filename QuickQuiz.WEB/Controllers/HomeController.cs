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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuickQuiz.Service.Services;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace QuickQuiz.WEB.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IMemberService _memberService;
        readonly ITestService _testService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IEmailService emailService, ITestService testService, IMemberService memberService) : base(userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _testService = testService;
            _memberService = memberService;
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

        //public IActionResult SignIn(SignInViewModel signInVM, SignUpViewModel signUpVM)
        //{
        //    var tuple = (new SignInViewModel(), new SignUpViewModel());
        //    return View(tuple);
        //}
        public IActionResult SignIn(SignUpViewModel signUpVM, bool signUpReturn = false)
        {
            if (!signUpReturn)
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
            var tuple = (new SignInViewModel(), signUpVM);
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
                return RedirectToAction(nameof(HomeController.SignIn), new { signUpVM = request, signUpReturn = true });
            AppUser user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                RegisterTime = DateTime.UtcNow
            };
            var identityResult = await _userManager.CreateAsync(user, request.PasswordConfirm);
            if (identityResult.Succeeded)
            {
                await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                string? callbackUrl = Url.Action("ConfirmEmail", "Home", new { token = encodedToken, userName = request.UserName, email = request.Email }, Request.Scheme, "quizck.com");
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
                user.AccountConfirmTime = DateTime.UtcNow;
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
            string passwordResetLink = Url.Action("PasswordChange", "Home",
                new { userId = hasUser.Id, token = passwordResetToken }, HttpContext.Request.Scheme);

            //örnek link : https://localhost:7295?userId?12213&token=dshgdfhsadsd  bu sekilde bir url üretilecek.
            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki, e-posta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(SignIn));
        }

        public async Task<IActionResult> PasswordChange(string userId, string token)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("UserNotFound", "Kullanıcı bulunamadı.");
                return View();
            }
            bool isVerify = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (!isVerify)
            {
                ModelState.AddModelError("TokenDenied", "Geçersiz token kullanımı yapılmıştır.");
                return View();
            }

            return View((new PasswordChangeViewModel() { Token=token}, userId));
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange( PasswordChangeViewModel viewModel, string userId)
        {
            if (!ModelState.IsValid)
                return View();
            if (userId != null)
            {
                if (viewModel.Token == null)
                {
                    ModelState.AddModelError("TokenDenied", "Geçersiz token kullanımı yapılmıştır.");
                    return View();
                }
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    ModelState.AddModelError("UserNotFound", "Kullanıcı bulunamadı.");
                    return View();
                }
                IdentityResult result = await _userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.PasswordNew);
                if (!result.Succeeded)
                    ModelState.AddModelErrorList(result.Errors);
                else
                {
                    TempData["SuccessMessage"] = "Şifreniz başarılı bir şekilde değiştirilmiştir.";
                    return RedirectToAction("SignIn");
                }
            }
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}