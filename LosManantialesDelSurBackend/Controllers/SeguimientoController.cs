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

        [HttpGet("{uuid}")]                         // Traer todos los seguimientos
        public async Task<ActionResult<List<Seguimiento>>> Get(string uuid) {
            var seguimiento = await context.Seguimiento.ToListAsync();
            return seguimiento;
        }

        [HttpPost]                                  // Generar seguimiento
        public async Task<ActionResult<string>> Post(Seguimiento seguimiento) {
            Guid uuid = Guid.NewGuid();
            seguimiento.Uuid = uuid.ToString();
            seguimiento.CreatedAt = DateTime.UtcNow;
            seguimiento.NumeroSeguimiento = generarCodigo();
            context.Seguimiento.Add(seguimiento);
            await context.SaveChangesAsync();
            return seguimiento.Uuid;
        }

        [HttpPut]                                   // Actualizar el seguimiento
        public async Task<ActionResult<int>> Update(Seguimiento seguimiento) {
            context.Seguimiento.Update(seguimiento);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]                                // Eliminar el seguimiento
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
            return codigo.Substring(0, 15).ToUpper();
        }

        /*================ ESTADOS SEGUIMIENTO ==================*/

        [HttpGet("estados/{uuid}")]                 // Traer todos los estados por seguimiento
        public async Task<ActionResult<List<EstadoSeguimiento>>> GetStatus(string uuid) {
            var estado = await context.EstadoSeguimiento.Where(x => x.Seguimiento == uuid).ToListAsync();
            return estado;
        }

        [HttpPost("estados")]                                  // Generar el estado del seguimiento
        public async Task<ActionResult<int>> PostStatus(EstadoSeguimiento estado) {
            context.EstadoSeguimiento.Add(estado);
            await context.SaveChangesAsync();
            return estado.Id;
        }
    }
}
