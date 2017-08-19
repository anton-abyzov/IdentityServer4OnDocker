using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;

namespace IdentityServer4.MVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _settings;

        public HomeController(IOptionsSnapshot<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        //[HttpGet]
        //[Authorize]
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

        [Authorize]
        [HttpGet("apicall")]
        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var apiService = _settings.ApiService;

            //var accessToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var accessToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync(apiService + "/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "Secure page.";

            return View();
        }

    }
}