using Microsoft.AspNetCore.Identity;

namespace QuickQuiz.WEB.Localization
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"Bu {userName} daha önce başka bir kullanıcı tarafından alınmıştır." };
            //return base.DuplicateUserName(userName);
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = $"Bu {email} daha önce başka bir kullanıcı tarafından alınmıştır." };
            //return base.DuplicateEmail(email);
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = $"Şifre en az 6 karakterli olmalıdır." };
            //return base.PasswordTooShort(length);
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new() { Code = "PasswordRequiresLower", Description = $"Şifreniz en az bir küçük harf içermelidir." };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new() { Code = "PasswordRequiresUpper", Description = $"Şifreniz en az bir büyük harf içermelidir." };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new() { Code = "PasswordRequiresUpper", Description = $"Şifreniz en az bir özel karakter içermelidir." };
        }
    }
}
