using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01DAW__2022RS650_2022JH650__Reservas.Models;

namespace P01DAW__2022RS650_2022JH650__Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservaController : ControllerBase
    {
        private readonly parqueoDBContext parqueoDBContexto;
        public reservaController(parqueoDBContext reservacontexto)
        {
            parqueoDBContexto = reservacontexto;
        }
        /// <summary>
        ///  Endpoint que retorna
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<reserva> listadoReserva = (from e in parqueoDBContexto.reserva
                                            select e).ToList();
            if (listadoReserva.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoReserva);
        }

        //Mostrar una lista de reservas activas del usuario.
        [HttpGet]
        [Route("ReservasActivas/{usuarioid}")]
        public IActionResult GetReservasActivas(int usuarioid)
        {
            var fechaActual = DateTime.Now.Date;
            var reservasActivas = from r in parqueoDBContexto.reserva
                                  where r.usuarioid == usuarioid &&
                                        r.fecha >= fechaActual &&
                                        r.estado == "Confirmada"
                                  select new
                                  {
                                      r.reservaid,
                                      r.fecha,
                                      r.hora,
                                      r.cant_hora,
                                      r.estado,
                                      r.parqueoid
                                  };

            return Ok(reservasActivas);
        }

        //Cancelar una reserva antes de su uso.
        [HttpPut]
        [Route("CancelarReserva/{reservaid}")]
        public IActionResult CancelarReserva(int reservaid)
        {
            var reserva = parqueoDBContexto.reserva.FirstOrDefault(r => r.reservaid == reservaid);
            if (reserva == null)
            {
                return NotFound("Reserva no encontrada.");
            }

            var fechaReserva = reserva.fecha;
            var horaReserva = TimeSpan.Parse(reserva.hora);
            var fechaHoraReserva = fechaReserva.Add(horaReserva);

            var parqueo = (from e in parqueoDBContexto.parqueo
                           where e.parqueoid == reserva.parqueoid
                           select e).FirstOrDefault();

            if (fechaHoraReserva <= DateTime.Now)
            {
                return BadRequest("No se puede cancelar una reserva pasada.");
            }

            if (parqueo == null)
            {
                return NotFound();
            }
            else
            {
                parqueo.estado = "Disponible";
            }

            reserva.estado = "Cancelada";
            parqueoDBContexto.SaveChanges();

            return Ok("Reserva cancelada exitosamente.");
        }
        

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarReserva([FromBody] reserva reserva)
        {
            try
            {
                //para controlar el identity en BD en AÑADIR
                reserva.reservaid = 0;
                reserva.estado = "Confirmada";

                var parqueo = (from e in parqueoDBContexto.parqueo
                               where e.parqueoid == reserva.parqueoid
                               select e).FirstOrDefault();
                if(parqueo == null)
                {
                    return NotFound();
                }
                else
                {
                    parqueo.estado = "Ocupado";
                    parqueoDBContexto.reserva.Add(reserva);
                    parqueoDBContexto.SaveChanges();
                }

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarReserva(int id, [FromBody] reserva reservaModificar)
        {
            reserva? equipoActual = (from e in parqueoDBContexto.reserva
                                     where e.reservaid == id
                                     select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }
            equipoActual.fecha = reservaModificar.fecha;
            equipoActual.hora = reservaModificar.hora;
            equipoActual.cant_hora = reservaModificar.cant_hora;
            equipoActual.estado = reservaModificar.estado;
            equipoActual.usuarioid = reservaModificar.usuarioid;
            equipoActual.parqueoid = reservaModificar.parqueoid;

            parqueoDBContexto.Entry(equipoActual).State = EntityState.Modified;
            parqueoDBContexto.SaveChanges();
            return Ok(reservaModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarReserva(int id)
        {
            reserva? reserva = (from e in parqueoDBContexto.reserva
                                where e.parqueoid == id
                                select e).FirstOrDefault();
            if (reserva == null)
            {
                return NotFound();

            }
            parqueoDBContexto.reserva.Attach(reserva);
            parqueoDBContexto.reserva.Remove(reserva);
            parqueoDBContexto.SaveChanges();

            return Ok(reserva);
        }
    }
}
