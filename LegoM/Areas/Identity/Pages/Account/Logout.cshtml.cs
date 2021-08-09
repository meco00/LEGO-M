using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LegoM.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LegoM.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
       

        public LogoutModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            
        }

       
        public async Task<IActionResult> OnGet()
        {
            
            await _signInManager.SignOutAsync();           

            return RedirectToAction("Index", "Home", new { area=""});
        }

       
    }
}
