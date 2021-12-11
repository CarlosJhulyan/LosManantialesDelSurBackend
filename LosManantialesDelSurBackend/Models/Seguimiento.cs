using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Seguimiento
    {
        public Seguimiento()
        {
            EstadoSeguimiento = new HashSet<EstadoSeguimiento>();
            Paquete = new HashSet<Paquete>();
        }

        public string Uuid { get; set; }
        public string NumeroSeguimiento { get; set; }
        public DateTime? FechEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<EstadoSeguimiento> EstadoSeguimiento { get; set; }
        public virtual ICollection<Paquete> Paquete { get; set; }
    }
}
