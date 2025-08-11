using EventosCostaRica.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosCostaRica.Models
{ 

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    public string Apellidos { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    public string Correo { get; set; }

    [ValidateNever] 
    public string ContraseñaHash { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; }

    public int RolId { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }

    [ValidateNever]
    public Rol Rol { get; set; }
}
}