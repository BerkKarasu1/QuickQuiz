using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Services;
using QuickQuiz.WEB.Extensions;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace QuickQuiz.WEB.Controllers
{
    [Authorize(Roles = "User,Admin")]
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

        public IActionResult PasswordChange()
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

            var (isSuccess, errors) =
                await _memberService.ChangePasswordAsync(userName, request.PasswordOld, request.PasswordNew);

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
            //ViewBag.educationLevel = new SelectList(Enum.GetNames(typeof(EducationLevel)));
            //ViewBag.occupation = new SelectList(Enum.GetNames(typeof(Occupation)));
            List<string> enums = new();
            List<string> enums2 = new();
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser!.UserName!,
                Email = currentUser!.Email!,
                BirthDate = currentUser!.BirthDate,
                Phone = currentUser!.PhoneNumber!,
                City = currentUser!.City,
                Gender = currentUser!.Gender,
                Github = currentUser!.Github,
                Twitter = currentUser!.Twitter,
                Instagram = currentUser!.Instagram,
                Facebook = currentUser!.Facebook,
                Linkedln = currentUser!.Linkedln,
                EducationLevel = currentUser.EducationLevel,
                EducationLevelDesc = currentUser.EducationLevel?.GetType().GetField(currentUser.EducationLevel?.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description,
                OccupationDesc = currentUser.Occupation?.GetType().GetField(currentUser.Occupation?.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description,
            };
            foreach (Occupation occupation in Enum.GetValues(typeof(Occupation)))
            {
                var desc = occupation.GetType().GetField(occupation.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description;
                if (desc != null)
                    enums.Add(desc);
            }
            foreach (EducationLevel educationLevel in Enum.GetValues(typeof(EducationLevel)))
            {
                var desc = educationLevel.GetType().GetField(educationLevel.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description;
                if (desc != null)
                    enums2.Add(desc);
            }
            ViewBag.occupation = new SelectList(enums);
            ViewBag.educationLevel = new SelectList(enums2);


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
                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Picture.FileName)}";
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
            foreach (EducationLevel educationLevel in Enum.GetValues(typeof(EducationLevel)))
            {
                var desc = educationLevel.GetType().GetField(educationLevel.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description;
                if (desc != null && desc == request.EducationLevelDesc)
                {
                    currentUser.EducationLevel = educationLevel;
                    break;
                }
            }
            foreach (Occupation occupation in Enum.GetValues(typeof(Occupation)))
            {
                var desc = occupation.GetType().GetField(occupation.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description;
                if (desc != null && desc == request.OccupationDesc)
                {
                    currentUser.Occupation = occupation;
                    break;
                }
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
                Facebook = currentUser.Facebook,
                OccupationDesc=request.OccupationDesc,
                EducationLevel=currentUser.EducationLevel,
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
            return View(await _memberService.GetUserViewModelByUserNameAsync(username));
        }
        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = "Lütfen mail adresinize gönderilen link üzerinden hesabınızı aktifleştiriniz.";
            ViewBag.message = message;
            return View();
        }
    }
}
