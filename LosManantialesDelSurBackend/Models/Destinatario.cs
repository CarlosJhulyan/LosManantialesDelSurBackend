using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Destinatario
    {
        public Destinatario()
        {
            Paquete = new HashSet<Paquete>();
        }

        public string Uuid { get; set; }
        public string Nombres { get; set; }
        public string Dni { get; set; }
        public string Celular { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Paquete> Paquete { get; set; }
    }
}
