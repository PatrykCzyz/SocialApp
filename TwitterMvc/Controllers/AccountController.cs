using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterMvc.Dtos;
using TwitterMvc.Dtos.AccountDtos;
using TwitterMvc.Models;

namespace TwitterMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto data)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(data.UserName);

                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, data.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto data)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser(data);

                var result = await _userManager.CreateAsync(user, data.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            var editDto = _mapper.Map<EditDto>(user);

            return View(editDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditDto editDto)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                user.UserName = editDto.UserName;
                user.Email = editDto.Email;
                user.Name = editDto.Name;
                user.Lastname = editDto.Lastname;
                user.Gender = editDto.Gender;
                user.Age = (int)editDto.Age;
                user.Country = editDto.Country;
                
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Profile");
            }

            return View(editDto);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
