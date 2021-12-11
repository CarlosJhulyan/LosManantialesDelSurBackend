using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Paquete = new HashSet<Paquete>();
            Pasaje = new HashSet<Pasaje>();
            Vehiculo = new HashSet<Vehiculo>();
        }

        public string Uuid { get; set; }
        public string Nombres { get; set; }
        public string Correo { get; set; }
        public string Pass { get; set; }
        public string Direccion { get; set; }
        public string Rol { get; set; }
        public string Dni { get; set; }
        public string Celular { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Paquete> Paquete { get; set; }
        public virtual ICollection<Pasaje> Pasaje { get; set; }
        public virtual ICollection<Vehiculo> Vehiculo { get; set; }
    }
}
