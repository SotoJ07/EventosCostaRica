using EventosCostaRica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventosCostaRica.Controllers;

public class EventosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public EventosController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var eventos = await _context.Eventos.ToListAsync();
        return View(eventos);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Evento evento, IFormFile imagen)
    {
        if (imagen != null && imagen.Length > 0)
        {
            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
            var rutaGuardado = Path.Combine(_env.WebRootPath, "imagenes", nombreArchivo);

            using (var stream = new FileStream(rutaGuardado, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            evento.ImagenUrl = "/imagenes/" + nombreArchivo;
        }

        if (!ModelState.IsValid)
            return View(evento);

        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();
        return View(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Evento evento, IFormFile imagen)
    {
        var original = await _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == evento.Id);
        if (original == null) return NotFound();

        if (imagen != null && imagen.Length > 0)
        {
            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
            var rutaGuardado = Path.Combine(_env.WebRootPath, "imagenes", nombreArchivo);

            using (var stream = new FileStream(rutaGuardado, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            evento.ImagenUrl = "/imagenes/" + nombreArchivo;
        }
        else
        {
            evento.ImagenUrl = original.ImagenUrl;
        }

        if (!ModelState.IsValid)
            return View(evento);

        _context.Eventos.Update(evento);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}