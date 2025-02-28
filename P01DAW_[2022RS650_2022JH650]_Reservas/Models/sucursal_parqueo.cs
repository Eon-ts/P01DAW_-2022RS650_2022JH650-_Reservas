using System.ComponentModel.DataAnnotations;

namespace P01DAW__2022RS650_2022JH650__Reservas.Models
{
    public class sucursal_parqueo
    {

        [Key]
        public int sucursalid { get; set; }
        public string? nombre_sucursal { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public int? administrador { get; set; }
        public int? nro_espacio_existente { get; set; }
        /*
    CREATE TABLE sucursal_parqueo (
    sucursalid INT IDENTITY PRIMARY KEY,
    nombre_sucursal VARCHAR(100),
    direccion VARCHAR(255),
    telefono VARCHAR(20),
    administrador INT,
    nro_espacio_existente INT
);
        */
    }
}
