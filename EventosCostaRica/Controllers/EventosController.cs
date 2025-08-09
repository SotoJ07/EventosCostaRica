using EventosCostaRica.Models;
using Microsoft.AspNetCore.Authorization;
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

    public async Task<IActionResult> Catalogo()
    {
        var eventos = await _context.Eventos.ToListAsync();
        return View(eventos);
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null)
            return NotFound();

        return View(evento);
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

        // Guardar evento
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();

        // Generar asientos asociados al evento
        var filas = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        int columnas = 10;

        foreach (var fila in filas)
        {
            for (int i = 1; i <= columnas; i++)
            {
                var asiento = new Asiento
                {
                    EventoId = evento.Id,
                    Fila = fila,
                    Numero = i,
                    EstaOcupado = false
                };
                _context.Asientos.Add(asiento);
            }
        }

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

    // Mostrar la vista de selección de asientos
    public async Task<IActionResult> SeleccionarAsientos(int eventoId)
    {
        var asientos = await _context.Asientos
            .Where(a => a.EventoId == eventoId)
            .OrderBy(a => a.Fila)
            .ThenBy(a => a.Numero)
            .Select(a => new AsientoViewModel
            {
                Id = a.Id,
                Fila = a.Fila,
                Numero = a.Numero,
                Ocupado = a.EstaOcupado,
                Seleccionado = false
            }).ToListAsync();

        ViewBag.EventoId = eventoId;

        return View(asientos);
    }

    // Confirmar selección
    /*[HttpPost]
    public async Task<IActionResult> ConfirmarAsientos(int eventoId, List<int> asientosSeleccionados)
    {
        if (asientosSeleccionados == null || !asientosSeleccionados.Any())
        {
            TempData["Error"] = "Debes seleccionar al menos un asiento.";
            return RedirectToAction("SeleccionarAsientos", new { eventoId });
        }

        var asientos = await _context.Asientos
            .Where(a => asientosSeleccionados.Contains(a.Id))
            .ToListAsync();

        foreach (var asiento in asientos)
        {
            asiento.EstaOcupado = true;
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "¡Asientos reservados exitosamente!";
        return RedirectToAction("DetalleEvento", new { id = eventoId });
    }*/
    [HttpPost]
    public async Task<IActionResult> ConfirmarAsientos(int eventoId, List<int> asientosSeleccionados)
    {
        var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
        var correoUsuario = HttpContext.Session.GetString("CorreoUsuario");

        if (asientosSeleccionados == null || !asientosSeleccionados.Any())
        {
            TempData["Error"] = "Debes seleccionar al menos un asiento.";
            return RedirectToAction("SeleccionarAsientos", new { eventoId });
        }

        var evento = await _context.Eventos.FindAsync(eventoId);
        if (evento == null) return NotFound();

        var asientos = await _context.Asientos
            .Where(a => asientosSeleccionados.Contains(a.Id))
            .OrderBy(a => a.Fila).ThenBy(a => a.Numero)
            .ToListAsync();

        var model = new ConfirmarCompraViewModel
        {
            Evento = evento,
            Asientos = asientos,
            UsuarioNombre = nombreUsuario ?? "Invitado",
            UsuarioCorreo = correoUsuario ?? "correo@ejemplo.com", // Modifica si usas claims
        };

        return View("ConfirmarCompra", model);
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarCompra(int eventoId, List<int> asientosSeleccionados)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        var evento = await _context.Eventos.FindAsync(eventoId);
        if (evento == null) return NotFound();

        if (asientosSeleccionados == null || !asientosSeleccionados.Any())
        {
            TempData["Error"] = "Debes seleccionar al menos un asiento.";
            return RedirectToAction("SeleccionarAsientos", new { eventoId });
        }

        var asientos = await _context.Asientos
            .Where(a => asientosSeleccionados.Contains(a.Id))
            .ToListAsync();

        var entrada = new Entrada
        {
            EventoId = eventoId,
            UsuarioId = usuarioId.Value,
            FechaCompra = DateTime.Now
        };
        _context.Entradas.Add(entrada);
        await _context.SaveChangesAsync();

        foreach (var asiento in asientos)
        {
            asiento.EstaOcupado = true;
            asiento.EntradaId = entrada.Id;
        }

        evento.Capacidad -= asientos.Count;

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "¡Compra confirmada!";
        return RedirectToAction("Detalle", new { id = eventoId });
    }
}