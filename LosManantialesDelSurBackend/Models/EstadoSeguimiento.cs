using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class EstadoSeguimiento
    {
        public int Id { get; set; }
        public string Seguimiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        public virtual Seguimiento SeguimientoNavigation { get; set; }
    }
}
