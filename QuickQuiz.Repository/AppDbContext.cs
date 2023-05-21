using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;
using System.Reflection;

namespace QuickQuiz.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=94.73.170.76;Initial Catalog=u0988076_db43A; User Id=u0988076_user43A;Password=B0-8i=AI-tc6v_L1; TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            builder.Entity<Question>()
                .HasMany(x => x.Answers).WithOne(x => x.Question);
            builder.Entity<Question>()
                  .HasMany(x => x.Tests).WithMany(x => x.Question)
                  .UsingEntity<Dictionary<string, object>>("TestQuestionManyToMany",
                  x => x.HasOne<Test>().WithMany().HasForeignKey("TestId").HasConstraintName("TestFK"),
                  x => x.HasOne<Question>().WithMany().HasForeignKey("QuestionId").HasConstraintName("QuestionFK"));
        }
    }
}