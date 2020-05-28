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

        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            CustomUser user = userId != null ? await _userManager.FindByIdAsync(userId) : await _userManager.GetUserAsync(User);
            var profile = new ProfileDto(user);

            ViewBag.UserId = user.Id;
            ViewBag.Avatar = $"/img/{profile.Gender}.png";
                
            var result = await _postService.GetPosts(user.Id);
            if (result.Succeeded)
            {
                ViewBag.Posts = result.Content;
            }
            else
            {
                ViewBag.Error = result.ErrorMessage;
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
