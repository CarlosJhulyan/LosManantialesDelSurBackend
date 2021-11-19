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

    public class SeguimientoController : ControllerBase {
        private readonly DBManantialesContext context;

        public SeguimientoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Seguimiento>>> Get() {
            var seguimiento = await context.Seguimiento.ToListAsync();
            return seguimiento;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(Seguimiento seguimiento) {
            Guid uuid = Guid.NewGuid();
            seguimiento.Uuid = uuid.ToString();
            seguimiento.CreatedAt = DateTime.UtcNow;
            seguimiento.NumeroSeguimiento = generarCodigo();
            context.Seguimiento.Add(seguimiento);
            await context.SaveChangesAsync();
            return seguimiento.Uuid;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Seguimiento seguimiento) {
            context.Seguimiento.Update(seguimiento);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var seguimiento = await context.Seguimiento.FindAsync(id);
            context.Seguimiento.Remove(seguimiento);
            await context.SaveChangesAsync();
            return Ok();
        }

        private string generarCodigo() {
            Guid uuid = Guid.NewGuid();
            var uuidString = uuid.ToString();
            var codigoArray = uuidString.Split('-');
            string codigo = string.Join("", codigoArray.Skip(1));
            return codigo.Substring(0, 15);
        }
    }
}
