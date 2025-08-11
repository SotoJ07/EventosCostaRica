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

        // GET: Guest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,LastName,Email,Password")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                guest.Role = "Guest"; // Auto-assign role
                _context.Add(guest);
                await _context.SaveChangesAsync();

                // ✅ Extra Debug
                System.Diagnostics.Debug.WriteLine($"✅ Guest Created: {guest.Id} - {guest.Name} {guest.LastName}");

                return RedirectToAction(nameof(Index)); // Go to list after creating
            }
            return View(guest);
        }

        // List all guests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Guests.ToListAsync());
        }
    }
}
