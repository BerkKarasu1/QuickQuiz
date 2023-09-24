using Exams.Service.Mapping;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Core.Services;
using QuickQuiz.Repository;
using QuickQuiz.Repository.Repositories;
using QuickQuiz.Service.Services;
using QuickQuiz.WEB.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentityWithExt();

builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie = new()
    {
        Name = "QuickQuizCookie",
        HttpOnly = false,
        SecurePolicy = CookieSecurePolicy.None,
        SameSite = SameSiteMode.Lax
    };
    opt.LoginPath = new PathString("/home/signin");
    opt.LogoutPath = new PathString("/Member/Logout");
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
}
);
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(QuestionMappingConfig).Assembly);
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<IMapper, Mapper>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");


app.Run();