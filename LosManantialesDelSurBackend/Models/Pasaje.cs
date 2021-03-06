using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Pasaje
    {
        public string Uuid { get; set; }
        public int? VehiculoPasaje { get; set; }
        public int? VehiculoPasajeStatic { get; set; }
        public string Pasajero { get; set; }
        public int? NumeroAsiento { get; set; }
        public int? OrigenSucursal { get; set; }
        public int? DestinoSucursal { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaLlegada { get; set; }
        public string CodigoValidacion { get; set; }
        public string NumeroGuia { get; set; }
        public double? MontoTotal { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Sucursal DestinoSucursalNavigation { get; set; }
        public virtual Sucursal OrigenSucursalNavigation { get; set; }
        public virtual Usuario PasajeroNavigation { get; set; }
        public virtual Vehiculo VehiculoPasajeNavigation { get; set; }
        public virtual Vehiculo VehiculoPasajeStaticNavigation { get; set; }
    }
}
