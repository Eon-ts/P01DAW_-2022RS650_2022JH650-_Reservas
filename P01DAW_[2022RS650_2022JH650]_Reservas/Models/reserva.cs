using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace P01DAW__2022RS650_2022JH650__Reservas.Models
{
    public class reserva
    {
        [Key]
        public int reservaid { get; set; }
        public DateTime fecha { get; set; }
        public string? hora { get; set; }
        public int? cant_hora { get; set; }
        public string? estado { get; set; }
        public int? usuarioid { get; set; }
        public int? parqueoid { get; set; }

        /*
        CREATE TABLE reserva(
    reservaid INT IDENTITY PRIMARY KEY,
    fecha DATE,
    hora VARCHAR(10),
    cant_hora INT,
    estado VARCHAR(20),
    usuarioid INT,
    parqueoid INT
);
            */
    }
}
