using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class CodigoValidacionPago
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public bool? Usado { get; set; }
    }
}
