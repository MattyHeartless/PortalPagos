//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortalPagos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PagosSinTimbrar
    {
        public string invoice { get; set; }
        public int id_invoice { get; set; }
        public string fecha { get; set; }
        public string fecha_timbrado { get; set; }
        public string tipo { get; set; }
        public int id_cliente { get; set; }
    }
}
