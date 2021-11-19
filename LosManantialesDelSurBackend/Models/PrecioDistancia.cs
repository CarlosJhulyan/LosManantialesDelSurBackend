using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class PrecioDistancia
    {
        public int Id { get; set; }
        public string OrigenSucursal { get; set; }
        public string DestinoSucursal { get; set; }
        public double? PrecioPasaje { get; set; }
        public double? PrecioPaquete { get; set; }
    }
}
