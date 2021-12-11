using LosManantialesDelSurBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Controllers {
    [Route("api/[controller]")]
    [ApiController]

    public class UsuarioController : ControllerBase {
        private readonly DBManantialesContext context;

        public UsuarioController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                       // Listar los usuarios
        public async Task<ActionResult<List<Usuario>>> Get() {
            var usuario= await context.Usuario.ToListAsync();
            return usuario;
        }

        [HttpGet("admins")]                                       // Listar solo personal administrativo
        public async Task<ActionResult<List<Usuario>>> GetAdmins() {
            var usuario = await context.Usuario.Where(x => x.Rol != "cliente").ToListAsync();
            return usuario;
        }

        [HttpGet("clientes")]                                       // Listar solo los clientes
        public async Task<ActionResult<List<Usuario>>> GetClients() {
            var usuario = await context.Usuario.Where(x => x.Rol == "cliente").ToListAsync();
            return usuario;
        }

        [HttpGet("{uuid}")]                             // Obtener un usuario por su uuid
        public async Task<ActionResult<Usuario>> Get(string uuid) {
            var user = await context.Usuario.FirstOrDefaultAsync(x => x.Uuid == uuid);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]                                      //crear nuevo usuario
        public async Task<ActionResult<Usuario>> Post(Usuario user) {
            Guid uuid = Guid.NewGuid();
            user.Uuid = uuid.ToString();
            user.CreatedAt = DateTime.UtcNow;
            context.Usuario.Add(user);
            await context.SaveChangesAsync();
            return Ok(new { message = "Usuario registrado correctamente.", data = user, statusCode = 201 });
        }
                    
        [HttpPut]                                        // Modificar usuario
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Update(Usuario user) {
            context.Usuario.Update(user);
            await context.SaveChangesAsync();
            return Ok(new { message = "Usuario modificado correctamente.", statusCode = 200 });
        }

        [HttpDelete]                                    // Eliminar usuario
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<int>> Delete(string uuid) {
            var user = await context.Usuario.FindAsync(uuid);
            context.Usuario.Remove(user);
            await context.SaveChangesAsync();
            return Ok(new { message = "Usuario eliminado correctamente.", statusCode = 200 });
        }
    }
}
