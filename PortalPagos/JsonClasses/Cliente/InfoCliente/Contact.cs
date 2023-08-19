using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalPagos.JsonClasses.Cliente.InfoCliente
{
    public class Contact
    {
        public int id { get; set; }
        public int clientId { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public object name { get; set; }
        public bool isBilling { get; set; }
        public bool isContact { get; set; }
        public List<Type> types { get; set; }
    }
}