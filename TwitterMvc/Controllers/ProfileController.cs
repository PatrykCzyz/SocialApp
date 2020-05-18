using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterMvc.Dtos;

namespace TwitterMvc.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var profile = new ProfileDto();

            var user = await _userManager.GetUserAsync(User);

            profile.Username = user.UserName;

            return View(profile);
        }
    }
}
