using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ServiceBusWeb.Models;

namespace ServiceBusWeb.Pages
{
    public class LoginModel : PageModel
    {
        #region Properties
        [BindProperty]
        public string Username { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }
        #endregion

        private readonly IConfiguration _configuration;
        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                Message = "Email and password fields are required.";
                return Page();
            }

            var user = _configuration.GetSection("SiteUser").Get<UserModel>();
            if (Username == user.Username)
            {
                var passwordHasher = new PasswordHasher<string>();
                //string hashedPassword = passwordHasher.HashPassword(Username, Password);
                if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Username),
                            new Claim("FullName", Username),
                            new Claim(ClaimTypes.Role, "Administrator")
                        };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties() { IsPersistent = true });
                    return LocalRedirect(GetLocalUrl(Url, returnUrl));
                }
            }
            Message = "Incorrect email or password.";
            return Page();
        }

        private string GetLocalUrl(IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                return urlHelper.Page("/Index");
            }
            return localUrl;
        }
    }
}
