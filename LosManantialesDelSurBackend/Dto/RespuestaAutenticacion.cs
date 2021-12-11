using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LosManantialesDelSurBackend.Dto {
    public class RespuestaAutenticacion {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
