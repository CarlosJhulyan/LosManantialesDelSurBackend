using LosManantialesDelSurBackend.Models;
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

        [HttpGet]                                           // Lista los usuarios
        public async Task<ActionResult<List<Usuario>>> Get() {
            var usuario = await context.Usuario.ToListAsync();
            return usuario;
        }

        [HttpGet("{uuid}")]                                 // Devuelve un usuario por su uuid
        public async Task<ActionResult<Usuario>> Get(string uuid) {
            var usuario = await context.Usuario.FirstOrDefaultAsync(x => x.Uuid == uuid);
	        if(usuario == null)
		        return NotFound();
	        return usuario;
        }

        [HttpPost]                                          // Registra un nuevo usuario
        public async Task<ActionResult<string>> Post(Usuario usuario) {
            Guid uuid = Guid.NewGuid();
            usuario.Uuid = uuid.ToString();
            usuario.CreatedAt = DateTime.UtcNow;
            context.Usuario.Add(usuario);
            await context.SaveChangesAsync();
            return usuario.Uuid;
        }

        [HttpPut]                                           // Actualiza los datos de un usuario
        public async Task<ActionResult<int>> Update(Usuario usuario) {
            context.Usuario.Update(usuario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]                                        // Elimina un usuario
        public async Task<ActionResult<int>> Delete(int id) {
            var usuario= await context.Usuario.FindAsync(id);
            context.Usuario.Remove(usuario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
