using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.MVCApplication.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }
      
    }
}