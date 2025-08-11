using System.Linq;
using System.Threading.Tasks;
using EventosCostaRica.Helpers;
using EventosCostaRica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventosCostaRica.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove(nameof(model.MensajeError));

            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == model.Correo);

            if (usuario == null)
            {
                model.MensajeError = "Correo no registrado.";
                return View(model);
            }

            if (!usuario.Estado)
            {
                model.MensajeError = "Usuario inactivo. Contacte al administrador.";
                return View(model);
            }

            if (!SeguridadHelper.VerificarPassword(model.Contraseña, usuario.ContraseñaHash))
            {
                model.MensajeError = "Contraseña incorrecta.";
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", $"{usuario.Nombre} {usuario.Apellidos}");
            HttpContext.Session.SetString("CorreoUsuario", usuario.Correo);
            HttpContext.Session.SetInt32("RolId", usuario.RolId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string firstName,
            string lastName,
            string email,
            string username,
            string password,
            string confirmPassword,
            bool acceptTerms)
        {
            if (!acceptTerms)
            {
                TempData["RegistrationMessage"] = "Debes aceptar los términos y condiciones.";
                return View();
            }

            if (password != confirmPassword)
            {
                TempData["RegistrationMessage"] = "Las contraseñas no coinciden.";
                return View();
            }

            if (await _context.Usuarios.AnyAsync(u => u.Correo == email))
            {
                TempData["RegistrationMessage"] = "El correo ya está registrado.";
                return View();
            }

            var guestRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Guest");
            if (guestRole == null)
            {
                TempData["RegistrationMessage"] = "No se encontró el rol de invitado.";
                return View();
            }

            var usuario = new Usuario
            {
                Nombre = firstName,
                Apellidos = lastName,
                Correo = email,
                ContraseñaHash = SeguridadHelper.HashPassword(password),
                RolId = guestRole.Id,
                Estado = true,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetInt32("RolId", guestRole.Id);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}