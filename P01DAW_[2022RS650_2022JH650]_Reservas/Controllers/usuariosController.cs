using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01DAW__2022RS650_2022JH650__Reservas.Models;

namespace P01DAW__2022RS650_2022JH650__Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly parqueoDBContext _parqueoDBContexto;
        public usuariosController(parqueoDBContext parqueoDBContexto)
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
            List<usuarios> listadoEquipo = (from e in _parqueoDBContexto.usuarios
                                           select e).ToList();
            if (listadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }
        /*
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            usuarios? equipo = (from e in _parqueoDBContexto.usuarios
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }*/
        [Route("Validar")]
        [HttpPost]
        public IActionResult ValidarCredenciales([FromBody] LoginN request)
        {
            try
            {
                var usuario = (from e in _parqueoDBContexto.usuarios
                               where e.nombre == request.Nombre && e.contrasenia == request.contrasenia
                               select e).FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound(new { mensaje = "Credenciales inválidas" });
                }
                else
                {
                    return Ok(new { mensaje = "Credenciales válidas" });

                }

                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
        //Para no retornar o pedir toda la info mejor se hace un modelo extra
        public class LoginN
        {
            public string Nombre { get; set; }
            public string contrasenia { get; set; }
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] usuarios usuario)
        {
            try
            {
                //para controlar el identity en BD 
                usuario.usuarioid = 0;
                _parqueoDBContexto.usuarios.Add(usuario);
                _parqueoDBContexto.SaveChanges();
                return Ok(usuario);
            }//Esto es para entender mejor el error(solo da mas info)
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] usuarios usuarioModificar)
        {
            usuarios? usuarioActual = (from e in _parqueoDBContexto.usuarios
                                     where e.usuarioid == id
                                     select e).FirstOrDefault();
            if (usuarioActual == null)
            {
                return NotFound();
            }
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.correo = usuarioModificar.correo;
            usuarioActual.telefono = usuarioModificar.telefono;
            usuarioActual.contrasenia = usuarioModificar.contrasenia;
            usuarioActual.rol = usuarioModificar.rol;

            _parqueoDBContexto.Entry(usuarioActual).State = EntityState.Modified;
            _parqueoDBContexto.SaveChanges();
            return Ok(usuarioModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            usuarios? equipo = (from e in _parqueoDBContexto.usuarios
                               where e.usuarioid == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();

            }
            _parqueoDBContexto.usuarios.Attach(equipo);
            _parqueoDBContexto.usuarios.Remove(equipo);
            _parqueoDBContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}
