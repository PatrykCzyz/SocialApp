using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using System.Linq;
using TwitterMvc.Dtos;

namespace TwitterMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usersList = await _context.CustomUsers
                .Select(x => new UserListItem(x))
                .ToListAsync();

            return View(usersList);
        }
    }
}
