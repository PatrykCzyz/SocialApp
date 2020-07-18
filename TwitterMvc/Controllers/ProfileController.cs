using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterMvc.Dtos;
using TwitterMvc.Dtos.UserDtos;
using TwitterMvc.Models;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<CustomUser> userManager, IPostService postService, IMapper mapper)
        {
            _userManager = userManager;
            _postService = postService;
            _mapper = mapper;
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
                TempData["Error"] = result.ErrorMessage;
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

        [Authorize]
        [HttpPost("EditPost")]
        public IActionResult EditPost(GetPostDto post)
        {
            var postData = _mapper.Map<PostDto>(post);
            ViewBag.PostId = post.Id;

            return View(postData);
        }

        [Authorize]
        [HttpPost("EditPost/{postId}")]
        public async Task<IActionResult> EditPost(int postId, PostDto postDto)
        {
            if(ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var result = await _postService.EditPost(userId, postId, postDto);

                if (!result.Succeeded)
                {
                    TempData["Error"] = result.ErrorMessage;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.PostId = postId;
            return View();
        }
        
        [Authorize]
        [HttpPost("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _postService.RemovePost(user.Id, postId);
        
            if (!result.Succeeded)
            {
                TempData["Error"] = result.ErrorMessage;
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
