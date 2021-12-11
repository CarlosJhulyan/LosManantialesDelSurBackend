using System;
using System.Collections.Generic;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class Paquete
    {
        public int Id { get; set; }
        public int? Vehiculo { get; set; }
        public string Remitente { get; set; }
        public string Seguimiento { get; set; }
        public string Destinatario { get; set; }
        public string NumeroGuia { get; set; }
        public string Descripcion { get; set; }
        public string Dimensiones { get; set; }
        public int? OrigenPaquete { get; set; }
        public int? DestinoPaquete { get; set; }
        public string CodigoValidacion { get; set; }
        public double? MontoTotal { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Destinatario DestinatarioNavigation { get; set; }
        public virtual Sucursal DestinoPaqueteNavigation { get; set; }
        public virtual Sucursal OrigenPaqueteNavigation { get; set; }
        public virtual Usuario RemitenteNavigation { get; set; }
        public virtual Seguimiento SeguimientoNavigation { get; set; }
        public virtual Vehiculo VehiculoNavigation { get; set; }
    }
}
