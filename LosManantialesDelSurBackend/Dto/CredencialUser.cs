using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Dto {
    public class CredencialUser {
        public string Correo { get; set; }

        public string Dni { get; set; }

        public string Nombres { get; set; }

        public string Uuid { get; set; }

        public string Rol { get; set; }

        public string Password { get; set; }
    }
}
