﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using System.Linq;
using TwitterMvc.Dtos;

namespace TwitterMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IdentityDatabaseContext _context;

        public HomeController(IdentityDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usersList = await _context.Users
                .Select(x => new UserListItem(x))
                .ToListAsync();

            return View(usersList);
        }
    }
}
