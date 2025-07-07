using System.ComponentModel.DataAnnotations;

namespace EventosCostaRica.Models;

public class Evento
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [Required, StringLength(300)]
    public string Descripcion { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }

    [Required]
    public DateTime FechaFin { get; set; }

    [Required, StringLength(100)]
    public string Lugar { get; set; }

    [Required]
    public decimal Precio { get; set; }

    public string? ImagenUrl { get; set; }
}