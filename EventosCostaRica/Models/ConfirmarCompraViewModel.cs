namespace EventosCostaRica.Models
{
    public class ConfirmarCompraViewModel
    {
        public Evento Evento { get; set; }
        public List<Asiento> Asientos { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioCorreo { get; set; }

        public string AsientosResumen => string.Join(", ", Asientos.OrderBy(a => a.Fila).ThenBy(a => a.Numero).Select(a => $"{a.Fila}{a.Numero}"));
    }
}
