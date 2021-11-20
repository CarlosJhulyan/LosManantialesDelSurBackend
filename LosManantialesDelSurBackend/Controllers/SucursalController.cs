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

    public class SucursalController : ControllerBase {
        private readonly DBManantialesContext context;

        public SucursalController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]                                       // Lista los sucursales
        public async Task<ActionResult<List<Sucursal>>> Get() {
            var sucursal = await context.Sucursal.ToListAsync();
            return sucursal;
        }

        [HttpPost]                                      // Añadir Sucursales
        public async Task<ActionResult<int>> Post(Sucursal sucursal) {
            context.Sucursal.Add(sucursal);
            await context.SaveChangesAsync();
            return sucursal.Id;
        }

        [HttpPut]                                       // Actualiza la sucursal
        public async Task<ActionResult<int>> Update(Sucursal sucursal) {
            context.Sucursal.Update(sucursal);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]                                    // Elimina la sucursal
        public async Task<ActionResult<int>> Delete(int id) {
            var sucursal = await context.Sucursal.FindAsync(id);
            context.Sucursal.Remove(sucursal);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
