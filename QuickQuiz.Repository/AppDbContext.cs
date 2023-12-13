using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration configuration;
        public AppDbContext()
        {
                
        }
        public AppDbContext(IConfiguration configuration):this()
        {
            this.configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var str = configuration.GetSection("ConnectionStrings").GetSection("SqlCon").Value;
            optionsBuilder.UseNpgsql(str);
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