using Microsoft.AspNetCore.Identity;
using QuickQuiz.Core.Model;
using Microsoft.AspNetCore.Mvc;
namespace QuickQuiz.WEB.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> userManager { get; }
        protected AppUser? CurrentUser => User?.Identity?.Name!=null? userManager.FindByNameAsync(User.Identity.Name).Result:null;
        public BaseController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
    }
}
