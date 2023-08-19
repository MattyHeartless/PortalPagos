using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalPagos.JsonClasses.Invoices.InvoiceDetails
{
    public class Item
    {
        public int id { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
        public double total { get; set; }
        public object unit { get; set; }
        public object tax1Id { get; set; }
        public object tax2Id { get; set; }
        public object tax3Id { get; set; }
        public int serviceId { get; set; }
        public object serviceSurchargeId { get; set; }
        public object productId { get; set; }
        public object feeId { get; set; }
        public object discountPrice { get; set; }
        public object discountQuantity { get; set; }
        public object discountTotal { get; set; }
    }
}