using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EventosCostaRica.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EventosCostaRica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var nombre = HttpContext.Session.GetString("NombreUsuario");
            ViewBag.NombreUsuario = nombre;

            // Obtener los eventos de la base de datos
            var eventos = _context.Eventos.ToList();

            // Pasar los eventos como modelo a la vista
            return View(eventos);
        }

        public IActionResult Seguridad(string filtro)
        {
            if (!UsuarioTieneRol("Administrador"))
                return RedirectToAction("AccesoDenegado");

            var usuarios = _context.Usuarios
                                   .Include(u => u.Rol)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                usuarios = usuarios.Where(u => u.Nombre.Contains(filtro) || u.Correo.Contains(filtro));
            }

            return View(usuarios.ToList());
        }

        private bool UsuarioTieneRol(string v)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditMainPage()
        {
            return View();
        }
    }
}