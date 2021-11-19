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

    public class ClienteController : ControllerBase {
        private readonly DBManantialesContext context;

        public ClienteController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Get() {
            var cliente = await context.Cliente.ToListAsync();
            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(Cliente cliente) {
            Guid uuid = Guid.NewGuid();
            cliente.Uuid = uuid.ToString();
            cliente.CreatedAt = DateTime.UtcNow;
            context.Cliente.Add(cliente);
            await context.SaveChangesAsync();
            return cliente.Uuid;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Cliente cliente) {
            context.Cliente.Update(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var cliente = await context.Cliente.FindAsync(id);
            context.Cliente.Remove(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
