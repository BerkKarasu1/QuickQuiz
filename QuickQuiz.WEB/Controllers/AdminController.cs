using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickQuiz.Repository;
using System.Data;

namespace QuickQuiz.WEB.Controllers
{
    public class AdminController:Controller
    {
        AppDbContext _context;
        public AdminController(AppDbContext appDbContext)
        {
           _context = appDbContext;
        }
        //[Authorize(Roles = "Manager,Admin")]
        public IActionResult Index()
        {
            _context.Roles.Add(new() { Id = "4375f941-f2cc-483b-9140-d5bb5da2ac80", Name = "Admin", NormalizedName = "Admin", ConcurrencyStamp = "asd" });
            var user = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == "berk57");
            _context.UserRoles.Add(new() { RoleId = "4375f941-f2cc-483b-9140-d5bb5da2ac80", UserId = user.Id });
            _context.SaveChanges();
            var users = _context.Users.ToList();
            return View();
        }
    }
}
