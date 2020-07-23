using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using System.Linq;
using TwitterMvc.Dtos.UserDtos;
using TwitterMvc.Models;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IFollowService _followService;

        public HomeController(AppDbContext context, UserManager<CustomUser> userManager, IFollowService followService)
        {
            _context = context;
            _userManager = userManager;
            _followService = followService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            if(loggedUser != null)
            {
                var following = await _followService.GetFollowing(loggedUser.Id);

                if (following.Succeeded)
                    ViewBag.Following = following.Content;
                else
                    ViewBag.Error = following.ErrorMessage;
            }

            var usersList = await _context.CustomUsers
                .Select(x => new UserListItemDto(x))
                .ToListAsync();

            return View(usersList);
        }
    }
}
