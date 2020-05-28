using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterMvc.Dtos;
using TwitterMvc.Models;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly IPostService _postService;

        public ProfileController(UserManager<CustomUser> userManager, IPostService postService)
        {
            _userManager = userManager;
            _postService = postService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = new ProfileDto(user);

            ViewData["posts"] = (await _postService.GetPosts(user.Id)).Result;

            return View(profile);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var profile = new ProfileDto(user);

            ViewData["userId"] = user.Id;
            var posts = await _postService.GetPosts(user.Id);
            if (posts.Error != null)
            {
                ViewData["error"] = posts.Error;
            }
            else
            {
                ViewData["posts"] = posts.Result;
            }

            return View(profile);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                await _postService.CreatePost(userId, postDto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
