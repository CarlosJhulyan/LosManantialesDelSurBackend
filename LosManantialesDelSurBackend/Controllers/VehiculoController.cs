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

    public class VehiculoController : ControllerBase {
        private readonly DBManantialesContext context;

        public VehiculoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet("{id}")]                                           // Lista los vehiculos por sucursal
        public async Task<ActionResult<List<Vehiculo>>> Get(int id) {
            var vehiculo = await context.Vehiculo.Where(x => x.SucursalActual == id).ToListAsync();
            return vehiculo;
        }

        [HttpGet("asientos/{id}")]
        public async Task<ActionResult<int[]>> GetAsientosDisponibles(int id) {
            // TODO: Verificar si el vehiculo existe o no
            var pasajes = await context.Pasaje.Where(x => x.VehiculoPasaje == id).ToListAsync();
            int[] asientos = new int[18];
            foreach (var pasaje in pasajes)
                asientos[(int) pasaje.NumeroAsiento - 1] = 1;
            return asientos;
        }

        [HttpPost]                                                  //Registra un vehiculo
        public async Task<ActionResult<int>> Post(Vehiculo vehiculo) {
            vehiculo.CreatedAt = DateTime.UtcNow;
            context.Vehiculo.Add(vehiculo);
            await context.SaveChangesAsync();
            return vehiculo.Id;
        }

        [HttpPut]                                                   //Actualiza los datos del Vehiculo
        public async Task<ActionResult<int>> Update(Vehiculo vehiculo) {
            context.Vehiculo.Update(vehiculo);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]                                            // Elimina un vehiculos
        public async Task<ActionResult<int>> Delete(int id) {
            var vehiculo = await context.Vehiculo.FindAsync(id);
            context.Vehiculo.Remove(vehiculo);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
