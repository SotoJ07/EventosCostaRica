using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventosCostaRica.Models;
using EventosCostaRica.Helpers;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace EventosCostaRica.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
            return View(usuarios);
        }

        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
            ViewData["Estados"] = new SelectList(new[] {
                new { Value = true, Text = "Activo" },
                new { Value = false, Text = "Inactivo" }
            }, "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            // Debug: Log if Password is bound
            System.Diagnostics.Debug.WriteLine($"Password bound: {(usuario.Password != null ? "YES" : "NO")}");

            if (!ModelState.IsValid)
            {
                // Log all validation errors
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                System.Diagnostics.Debug.WriteLine("ModelState errors: " + errors);

                // Repopulate dropdowns
                ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
                ViewData["Estados"] = new SelectList(new[] {
            new { Value = true, Text = "Activo" },
            new { Value = false, Text = "Inactivo" }
        }, "Value", "Text", usuario.Estado);

                return View(usuario);
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Creating Usuario with password: " + usuario.Password);

                usuario.FechaRegistro = DateTime.Now;

                // Hash the password before saving
                usuario.ContraseñaHash = SeguridadHelper.HashPassword(usuario.Password);

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Usuario saved with ID: " + usuario.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception saving usuario: " + ex.Message);
                ModelState.AddModelError("", "Error saving user: " + ex.Message);
            }

            // Repopulate dropdowns on error
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            ViewData["Estados"] = new SelectList(new[] {
        new { Value = true, Text = "Activo" },
        new { Value = false, Text = "Inactivo" }
    }, "Value", "Text", usuario.Estado);
            return View(usuario);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            ViewData["Estados"] = new SelectList(new[] {
                new { Value = true, Text = "Activo" },
                new { Value = false, Text = "Inactivo" }
            }, "Value", "Text", usuario.Estado);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario, string? nuevaContraseña)
        {
            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDb = await _context.Usuarios.FindAsync(id);
                    if (usuarioDb == null) return NotFound();

                    usuarioDb.Nombre = usuario.Nombre;
                    usuarioDb.Apellidos = usuario.Apellidos;
                    usuarioDb.Correo = usuario.Correo;
                    usuarioDb.RolId = usuario.RolId;
                    usuarioDb.Estado = usuario.Estado;

                    if (!string.IsNullOrWhiteSpace(nuevaContraseña))
                    {
                        usuarioDb.ContraseñaHash = SeguridadHelper.HashPassword(nuevaContraseña);
                    }

                    _context.Update(usuarioDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(u => u.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            ViewData["Estados"] = new SelectList(new[] {
                new { Value = true, Text = "Activo" },
                new { Value = false, Text = "Inactivo" }
            }, "Value", "Text", usuario.Estado);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
