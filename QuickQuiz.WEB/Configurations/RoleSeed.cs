using QuickQuiz.Core.Model;
using QuickQuiz.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.WEB.Configurations
{
    public static class RoleSeed
    {
        public static void Seed(WebApplication application)
        {
            using (var serviceScope = application.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new AppRole { Name = "Admin", NormalizedName = "ADMIN" },
                        new AppRole { Name = "User", NormalizedName = "USER" });
                    context.SaveChanges();
                }
            }
        }
    }
}
