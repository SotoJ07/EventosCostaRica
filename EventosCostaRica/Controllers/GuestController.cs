using EventosCostaRica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventosCostaRica.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GuestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,LastName,Email,Password")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                guest.Role = "Guest";
                _context.Add(guest);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Guest Created: {guest.Id} - {guest.Name} {guest.LastName}");

                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Guests.ToListAsync());
        }
    }
}
