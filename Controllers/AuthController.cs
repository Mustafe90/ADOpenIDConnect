using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;

namespace ADOpenIDConnect.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login(string ReturnUrl)
        {
            IdentityModelEventSource.ShowPII = true;


            return Challenge(new AuthenticationProperties
            {
                RedirectUri = ReturnUrl
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task  Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //This bad boy will redirect us to the url we specified  open id config.SignedOutRedirectUri
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            }
        }
    }
}
