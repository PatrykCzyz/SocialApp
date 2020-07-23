using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterMvc.Models;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Controllers
{
    [Route("[controller]")]
    public class FollowController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly IFollowService _followService;
        private readonly IMapper _mapper;

        public FollowController(UserManager<CustomUser> userManager, IFollowService followService, IMapper mapper)
        {
            _userManager = userManager;
            _followService = followService;
            _mapper = mapper;
        }

        [HttpPost("Follow/{userToFollowId}")]
        public async Task<IActionResult> Follow(string userToFollowId)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            await _followService.Follow(userId, userToFollowId);

            return RedirectToAction("Index", "Profile", new { userId = userToFollowId});
        }

        [HttpPost("UnFollow/{userToUnFollowId}")]
        public async Task<IActionResult> UnFollow(string userToUnFollowId)
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            await _followService.UnFollow(userId, userToUnFollowId);

            return RedirectToAction("Index", "Profile", new { userId = userToUnFollowId });
        }
    }
}
