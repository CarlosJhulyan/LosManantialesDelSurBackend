using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Cliente
    {
        public string Uuid { get; set; }
        public string Nombres { get; set; }
        public string Correo { get; set; }
        public string Dni { get; set; }
        public string Celular { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
