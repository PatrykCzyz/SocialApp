using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterMvc.Dtos;

namespace TwitterMvc.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = new ProfileDto(user);

            return View(profile);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var profile = new ProfileDto(user);

            return View(profile);
        }
    }
}
