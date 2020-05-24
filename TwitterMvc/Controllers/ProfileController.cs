using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
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

            return View(profile);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var profile = new ProfileDto(user);

            ViewData["userId"] = userId;

            return View(profile);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            if (ModelState.IsValid)
            {
                await _postService.CreatePost(postDto);
            }

            return View(nameof(Index));
        }
    }
}
