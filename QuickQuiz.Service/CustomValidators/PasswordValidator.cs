using Microsoft.AspNetCore.Identity;
using QuickQuiz.Core.Model;

namespace QuickQuiz.Service.CustomValidators
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new() { Code = "PasswordContainUserName", Description = "Şifre alanı kullanıcı adı içeremez" });
            }
            if (password!.ToLower().StartsWith("123") || password!.ToLower().Contains("123")
                || password!.ToLower().Contains("1234") || password!.ToLower().Contains("1234")
                || password!.ToLower().Contains("12345") || password!.ToLower().Contains("12345")
                || password!.ToLower().Contains("123456") || password!.ToLower().Contains("123456"))
            {
                errors.Add(new() { Code = "PasswordNotContain1234", Description = "Şifre alanı ardışık sayı içeremez" });
            }
            if (password!.ToLower().StartsWith("abc") || password!.ToLower().StartsWith("abc")
                || password!.ToLower().Contains("abcd") || password!.ToLower().Contains("abcd")
                || password!.ToLower().Contains("abcde") || password!.ToLower().Contains("abcde")
                || password!.ToLower().Contains("abcdef") || password!.ToLower().Contains("abcdef"))
            {
                errors.Add(new() { Code = "PasswordNotContainabcd", Description = "Şifre alanı ardışık harf içeremez" });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
