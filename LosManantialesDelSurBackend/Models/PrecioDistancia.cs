using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class PrecioDistancia
    {
        public int Id { get; set; }
        public int? OrigenSucursal { get; set; }
        public int? DestinoSucursal { get; set; }
        public double? PrecioPasaje { get; set; }
        public double? PrecioPaquete { get; set; }

        public virtual Sucursal DestinoSucursalNavigation { get; set; }
        public virtual Sucursal OrigenSucursalNavigation { get; set; }
    }
}
