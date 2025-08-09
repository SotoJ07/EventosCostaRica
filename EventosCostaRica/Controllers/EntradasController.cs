using EventosCostaRica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventosCostaRica.Controllers
{
    public class EntradasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntradasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var entradas = await _context.Entradas
                .Include(e => e.Evento)
                .Include(e => e.Asientos)
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();

            return View(entradas);
        }
    }
}
