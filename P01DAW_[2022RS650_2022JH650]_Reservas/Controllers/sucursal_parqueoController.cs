using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01DAW__2022RS650_2022JH650__Reservas.Models;

namespace P01DAW__2022RS650_2022JH650__Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sucursal_parqueoController : ControllerBase
    {
        private readonly parqueoDBContext _parqueoDBContexto;
        public sucursal_parqueoController(parqueoDBContext parqueoDBContexto)
        {
            _parqueoDBContexto = parqueoDBContexto;
        }
        /// <summary>
        ///  Endpoint que retorna
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadosucursal = (from s in _parqueoDBContexto.sucursal_parqueo
                                   join p in _parqueoDBContexto.parqueo
                                       on s.sucursalid equals p.sucursalid into grupo
                                   select new
                                   {
                                       Sucursal = s,
                                       CantidadParqueos = grupo.Count()
                                   }).ToList();
                                    //Para que solo muestre los parqueos existentes ocupamos count para que sea dinamico y mas facil controlar

            if (listadosucursal.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadosucursal);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] sucursal_parqueo sucursal)
        {
            try
            {
                //Para controlar el identity.
                sucursal.sucursalid = 0;

                _parqueoDBContexto.sucursal_parqueo.Add(sucursal);
                _parqueoDBContexto.SaveChanges();
                return Ok(sucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] sucursal_parqueo sucursalModificar)
        {
            sucursal_parqueo? sucursalActual = (from e in _parqueoDBContexto.sucursal_parqueo
                                     where e.sucursalid == id
                                     select e).FirstOrDefault();
            if (sucursalActual == null)
            {
                return NotFound();
            }
            sucursalActual.nombre_sucursal = sucursalModificar.nombre_sucursal;
            sucursalActual.direccion = sucursalModificar.direccion;
            sucursalActual.telefono = sucursalModificar.telefono;
            sucursalActual.administrador = sucursalModificar.administrador;

            _parqueoDBContexto.Entry(sucursalActual).State = EntityState.Modified;
            _parqueoDBContexto.SaveChanges();
            return Ok(sucursalModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            sucursal_parqueo? equipo = (from e in _parqueoDBContexto.sucursal_parqueo
                               where e.sucursalid == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();

            }
            _parqueoDBContexto.sucursal_parqueo.Attach(equipo);
            _parqueoDBContexto.sucursal_parqueo.Remove(equipo);
            _parqueoDBContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}
