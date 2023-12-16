using Microsoft.AspNetCore.Identity;
using QuickQuiz.Core.Model;
using QuickQuiz.Repository;
using QuickQuiz.Service.CustomValidators;
using QuickQuiz.WEB.Localization;

namespace QuickQuiz.WEB.Extensions
{
    public static class StartupExtension
    {
        public static void AddIdentityWithExt(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);  // Password token ömrü 2 saat olarak belirlendi
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_-*.";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // 3 dk boyunca kitlenecek.
                options.Lockout.MaxFailedAccessAttempts = 3;  // 3 başarısız girişte
                options.Lockout.AllowedForNewUsers = true; 

            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
            // (servis olarak Identity eklendi. bu kod program.cs den taşındı!!)
        }
    }
}
