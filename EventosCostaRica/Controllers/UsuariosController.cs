using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventosCostaRica.Models;

namespace EventosCostaRica.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string filtro)
        {
            return RedirectToAction("Seguridad", "Home", new { filtro });
        }

        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaRegistro = DateTime.Now;
                usuario.Estado = true;
                usuario.ContraseñaHash = usuario.ContraseñaHash; // Reemplazar con hashing real si aplica
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Seguridad", "Home");
            }

            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(u => u.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction("Seguridad", "Home");
            }

            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Seguridad", "Home");
        }
    }
}