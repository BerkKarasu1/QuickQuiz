using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickQuiz.Core.Model;

namespace QuickQuiz.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server= DESKTOP-R25IHH5\\MSSQLSERVER044; database= QuickQuizDB; integrated security = true;");
        }
    }
}