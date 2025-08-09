namespace EventosCostaRica.Models
{
    public class Asiento
    {
        public int Id { get; set; }
        public string Fila { get; set; }
        public int Numero { get; set; }
        public bool EstaOcupado { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public int? EntradaId { get; set; }
        public Entrada Entrada { get; set; }
    }
}
