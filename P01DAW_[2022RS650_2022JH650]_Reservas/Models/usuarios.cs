using System.ComponentModel.DataAnnotations;
namespace P01DAW__2022RS650_2022JH650__Reservas.Models
{
    public class usuarios
    {
        [Key]
        public int usuarioid { get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? contrasenia { get; set; }
        public string? rol { get; set; }
    }
}
