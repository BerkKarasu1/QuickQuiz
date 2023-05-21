using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using QuickQuiz.WEB.Extensions;

namespace QuickQuiz.WEB.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        private readonly IMemberService _memberService;
        private string userName => User.Identity!.Name!;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IMemberService memberService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _memberService.GetUserViewModelByUserNameAsync(userName));
        }

        public async Task<IActionResult> PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!await _memberService.CheckPasswordAsync(userName, request.PasswordOld))
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış!");
                return View();
            }

            var(isSuccess, errors) = await _memberService.ChangePasswordAsync(userName, request.PasswordOld, request.PasswordNew);

            if (!isSuccess)
            {
                ModelState.AddModelErrorList(errors!);
                return View();
            }

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir.";

            return View();
        }

        public async Task Logout()
        {
            await _memberService.LogoutAsync();
        }

        public async Task<IActionResult> UserEdit()
        {
            var currentUser = await _userManager.FindByNameAsync(User!.Identity!.Name!);
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser!.UserName!,
                Email = currentUser!.Email!,
                BirthDate = currentUser!.BirthDate,
                Phone= currentUser!.PhoneNumber!,
                City = currentUser!.City,
                Gender = currentUser!.Gender,
                Github= currentUser!.Github,
                Twitter = currentUser!.Twitter,
                Instagram = currentUser!.Instagram,
                Facebook = currentUser!.Facebook,
                Linkedln = currentUser!.Linkedln
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            currentUser!.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;
            currentUser.Github = request.Github;
            currentUser.Facebook = request.Facebook;
            currentUser.Instagram = request.Instagram;
            currentUser.Linkedln = request.Linkedln;
            currentUser.Twitter = request.Twitter;


            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "Üye bilgileri başarıyla değiştirilmiştir.";

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender,
                Github = currentUser.Github,
                Twitter = currentUser.Twitter,
                Instagram = currentUser.Instagram,
                Linkedln = currentUser.Linkedln,
                Facebook = currentUser.Facebook
            };

            return View(userEditViewModel);
        }
        [Route("Member/UserSearch/{name?}")]
        public async Task<IActionResult> UserSearch(string name)
        {
            return View(await _memberService.GetUserViewModelBySearchedAsync(name));
        }
        [HttpGet]
        [Route("Member/UserProfile/{username?}")]
        public async Task<IActionResult> UserProfile(string username)
        {
            return View(await _memberService.GetUserViewModelByUserNameAsync(userName));
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            message = "Bu sayfayı görmeye yetkiniz yoktur. Yetki almak için yöneticiniz ile görüşebilirsiniz.";
            ViewBag.message = message;
            return View();
        }
    }
}
