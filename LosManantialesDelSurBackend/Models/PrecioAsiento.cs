using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class PrecioAsiento
    {
        public int Id { get; set; }
        public int? NumeroAsiento { get; set; }
        public int? PorcentajeVariacion { get; set; }
    }
}
