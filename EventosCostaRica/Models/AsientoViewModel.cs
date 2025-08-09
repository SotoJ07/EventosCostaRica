namespace EventosCostaRica.Models
{
    public class AsientoViewModel
    {
        public int Id { get; set; }
        public string Fila { get; set; } = "";
        public int Numero { get; set; }
        public bool Ocupado { get; set; }
        public bool Seleccionado { get; set; }
    }
}
