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
    
    public partial class Pagos
    {
        public int Id_Pago { get; set; }
        public int id_invoice { get; set; }
        public int id_cliente { get; set; }
        public string invoice { get; set; }
        public Nullable<decimal> monto { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public Nullable<System.DateTime> fecha_voucher { get; set; }
        public string tipo { get; set; }
    }
}