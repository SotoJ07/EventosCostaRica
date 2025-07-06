namespace EventosCostaRica.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Use "Admin" and "Guest"

        public ICollection<Usuario> Usuarios { get; set; }
    }
}