using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Vehiculo
    {
        public Vehiculo()
        {
            Paquete = new HashSet<Paquete>();
            Pasaje = new HashSet<Pasaje>();
        }

        public int Id { get; set; }
        public string Conductor { get; set; }
        public string Placa { get; set; }
        public bool? Estado { get; set; }
        public int? SucursalActual { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? SucursalFinal { get; set; }

        public virtual Usuario ConductorNavigation { get; set; }
        public virtual Sucursal SucursalActualNavigation { get; set; }
        public virtual Sucursal SucursalFinalNavigation { get; set; }
        public virtual ICollection<Paquete> Paquete { get; set; }
        public virtual ICollection<Pasaje> Pasaje { get; set; }
    }
}
