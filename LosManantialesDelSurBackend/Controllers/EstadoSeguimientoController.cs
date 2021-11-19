﻿using LosManantialesDelSurBackend.Models;
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

    public class EstadoSeguimientoController : ControllerBase {
        private readonly DBManantialesContext context;

        public EstadoSeguimientoController(DBManantialesContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EstadoSeguimiento>>> Get() {
            var estado = await context.EstadoSeguimiento.ToListAsync();
            return estado;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(EstadoSeguimiento estado) {
            context.EstadoSeguimiento.Add(estado);
            await context.SaveChangesAsync();
            return estado.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(EstadoSeguimiento estado) {
            context.EstadoSeguimiento.Update(estado);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id) {
            var estado = await context.EstadoSeguimiento.FindAsync(id);
            context.EstadoSeguimiento.Remove(estado);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
