using System.ComponentModel.DataAnnotations;

namespace EventosCostaRica.Models
{
    public class Entrada
    {
        public int Id { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public DateTime FechaCompra { get; set; }


        public Evento Evento { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Asiento> Asientos { get; set; }
    }
}
