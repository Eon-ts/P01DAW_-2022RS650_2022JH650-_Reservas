using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01DAW__2022RS650_2022JH650__Reservas.Models;
using System.Security.Cryptography;

namespace P01DAW__2022RS650_2022JH650__Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class parqueoController : ControllerBase
    {
        private readonly parqueoDBContext parqueoDBContexto;
        public parqueoController(parqueoDBContext equiposContexto)
        {
            parqueoDBContexto = equiposContexto;
        }
        /// <summary>
        ///  Endpoint que retorna
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<parqueo> listadoParqueo = (from e in parqueoDBContexto.parqueo
                                           select e).ToList();
            if (listadoParqueo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoParqueo);
        }

        //Mostrar una lista de todos los espacios de parqueo disponibles para reservar por día.
        [HttpGet]
        [Route("Disponibles/{fecha_AAAA_MM_DD}")]
        //Fecha formato AAAA-MM-DD
        public IActionResult GetDisponibles(DateTime fecha_AAAA_MM_DD)
        {
            var disponibles = from p in parqueoDBContexto.parqueo
                              where p.estado == "Disponible" &&
                                    !(from r in parqueoDBContexto.reserva
                                      where r.fecha == fecha_AAAA_MM_DD
                                      select r.parqueoid)
                                      .Contains(p.parqueoid)
                              select p;

            return Ok(disponibles);
        }


        //Mostrar una lista de los espacios reservados por día de todas las sucursales.
        [HttpGet]
        [Route("Reservados/{fecha}")]
        public IActionResult GetReservados(DateTime fecha)
        {
            var reservados = from r in parqueoDBContexto.reserva
                             join p in parqueoDBContexto.parqueo on r.parqueoid equals p.parqueoid
                             join s in parqueoDBContexto.sucursal_parqueo on p.sucursalid equals s.sucursalid
                             where r.fecha == fecha &&
                                   p.estado == "Ocupado"
                             select new
                             {
                                 r.reservaid,
                                 r.fecha,
                                 r.hora,
                                 r.cant_hora,
                                 r.estado,
                                 p.numero,
                                 p.ubicacion,
                                 s.nombre_sucursal
                             };

            return Ok(reservados);
        }


        //Mostrar una lista de los espacios reservados entre dos fechas dadas de una sucursal especifica.
        [HttpGet]
        [Route("Reservados/{sucursalid}/{fechaInicio}/{fechaFin}")]
        public IActionResult GetReservadosPorSucursal(int sucursalid, DateTime fechaInicio, DateTime fechaFin)
        {
            var reservados = from r in parqueoDBContexto.reserva
                             join p in parqueoDBContexto.parqueo on r.parqueoid equals p.parqueoid
                             where p.sucursalid == sucursalid &&
                                   r.fecha >= fechaInicio &&
                                   r.fecha <= fechaFin &&
                                   p.estado == "Ocupado"
                             select new
                             {
                                 r.reservaid,
                                 r.fecha,
                                 r.hora,
                                 r.cant_hora,
                                 r.estado,
                                 p.numero,
                                 p.ubicacion
                             };

            return Ok(reservados);
        }
        

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarParqueo([FromBody] parqueo parqueo)
        {
            try
            {
                //para controlar el identity en BD en AÑADIR
                parqueo.parqueoid = 0;
                parqueoDBContexto.parqueo.Add(parqueo);
                parqueoDBContexto.SaveChanges();
                return Ok(parqueo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarParqueo(int id, [FromBody] parqueo parqueoModificar)
        {
            parqueo? equipoActual = (from e in parqueoDBContexto.parqueo
                                     where e.parqueoid == id
                                     select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }
            equipoActual.numero = parqueoModificar.numero;
            equipoActual.ubicacion = parqueoModificar.ubicacion;
            equipoActual.costo_hora = parqueoModificar.costo_hora;
            equipoActual.estado = parqueoModificar.estado;
            equipoActual.sucursalid = parqueoModificar.sucursalid;

            parqueoDBContexto.Entry(equipoActual).State = EntityState.Modified;
            parqueoDBContexto.SaveChanges();
            return Ok(parqueoModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            parqueo? parqueo = (from e in parqueoDBContexto.parqueo
                               where e.parqueoid == id
                               select e).FirstOrDefault();
            if (parqueo == null)
            {
                return NotFound();

            }
            parqueoDBContexto.parqueo.Attach(parqueo);
            parqueoDBContexto.parqueo.Remove(parqueo);
            parqueoDBContexto.SaveChanges();

            return Ok(parqueo);
        }

    }
}
