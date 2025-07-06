using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EventosCostaRica.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string ContraseñaHash { get; set; }
        public int RolId { get; set; }
        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }


        public Rol Rol { get; set; }
    }
}