using System.ComponentModel.DataAnnotations;

namespace P01DAW__2022RS650_2022JH650__Reservas.Models
{
    public class parqueo
    {
        [Key]
        public int parqueoid { get; set; }
        public string? numero { get; set; }
        public string? ubicacion { get; set; }
        public decimal? costo_hora { get; set; }
        public string? estado { get; set; }
        public int? sucursalid { get; set; }


        /*
    parqueoid INT IDENTITY PRIMARY KEY,
    numero VARCHAR(50),
    ubicacion VARCHAR(255),
    costo_hora DECIMAL(10,2),
    estado VARCHAR(20),
    sucursalid INT
         */
    }
}
