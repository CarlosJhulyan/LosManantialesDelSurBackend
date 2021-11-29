using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Sucursal
    {
        public Sucursal()
        {
            PaqueteDestinoPaqueteNavigation = new HashSet<Paquete>();
            PaqueteOrigenPaqueteNavigation = new HashSet<Paquete>();
            PasajeDestinoSucursalNavigation = new HashSet<Pasaje>();
            PasajeOrigenSucursalNavigation = new HashSet<Pasaje>();
            PrecioDistanciaDestinoSucursalNavigation = new HashSet<PrecioDistancia>();
            PrecioDistanciaOrigenSucursalNavigation = new HashSet<PrecioDistancia>();
            VehiculoSucursalActualNavigation = new HashSet<Vehiculo>();
            VehiculoSucursalFinalNavigation = new HashSet<Vehiculo>();
        }

        public int Id { get; set; }
        public string CodigoPostal { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }

        public virtual ICollection<Paquete> PaqueteDestinoPaqueteNavigation { get; set; }
        public virtual ICollection<Paquete> PaqueteOrigenPaqueteNavigation { get; set; }
        public virtual ICollection<Pasaje> PasajeDestinoSucursalNavigation { get; set; }
        public virtual ICollection<Pasaje> PasajeOrigenSucursalNavigation { get; set; }
        public virtual ICollection<PrecioDistancia> PrecioDistanciaDestinoSucursalNavigation { get; set; }
        public virtual ICollection<PrecioDistancia> PrecioDistanciaOrigenSucursalNavigation { get; set; }
        public virtual ICollection<Vehiculo> VehiculoSucursalActualNavigation { get; set; }
        public virtual ICollection<Vehiculo> VehiculoSucursalFinalNavigation { get; set; }
    }
}
