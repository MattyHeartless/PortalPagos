using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace PortalPagos.Controllers.Cliente
{
    public class PagosController : Controller
    {
        // GET: Pagos

        public async Task<ActionResult> getPagos()
        {
            using (var clients = new HttpClient())
            {
                HttpClient client = new HttpClient();
                List<Root> list = new List<Root>();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                client.BaseAddress = new Uri("https://189.199.227.94/crm/api/v1.0/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-Auth-App-Key", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq");
                //                client.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue("X-Auth-App-Key", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("invoices?organizationId=2&clientId=4");
                if (response.IsSuccessStatusCode)
                {
                    var s = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Root>>(s);
                }

                List<Invoices> linv = new List<Invoices>();
                foreach (var item in list)
                {
                    Invoices inv = new Invoices();
                    inv.id = item.id;
                    inv.number = item.number;
                    inv.amountToPay = item.amountToPay;
                    inv.total = item.total;
                    inv.status = item.status;
                    inv.type = item.items[0].type;
                    inv.label = item.items[0].label;
                    linv.Add(inv);
                }

                return Json(linv);
            }

        }
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
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
            public double? discountPrice { get; set; }
            public double? discountQuantity { get; set; }
            public double? discountTotal { get; set; }
        }

        public class Invoices
        {
            public int id { get; set; }
            public string number { get; set; }
            public double amountToPay { get; set; }
            public double total { get; set; }
            public int status { get; set; }
            public string type { get; set; }
            public string label { get; set; }
        }

        public class PaymentCover
        {
            public int id { get; set; }
            public int invoiceId { get; set; }
            public int paymentId { get; set; }
            public object creditNoteId { get; set; }
            public object refundId { get; set; }
            public double amount { get; set; }
        }

        public class Root
        {
            public int id { get; set; }
            public int clientId { get; set; }
            public string number { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime dueDate { get; set; }
            public DateTime emailSentDate { get; set; }
            public int maturityDays { get; set; }
            public DateTime taxableSupplyDate { get; set; }
            public object notes { get; set; }
            public object adminNotes { get; set; }
            public List<Item> items { get; set; }
            public double subtotal { get; set; }
            public object discount { get; set; }
            public string discountLabel { get; set; }
            public List<object> taxes { get; set; }
            public double total { get; set; }
            public double amountPaid { get; set; }
            public double totalUntaxed { get; set; }
            public double totalDiscount { get; set; }
            public double totalTaxAmount { get; set; }
            public double amountToPay { get; set; }
            public string currencyCode { get; set; }
            public int status { get; set; }
            public List<PaymentCover> paymentCovers { get; set; }
            public int invoiceTemplateId { get; set; }
            public int proformaInvoiceTemplateId { get; set; }
            public string organizationName { get; set; }
            public object organizationRegistrationNumber { get; set; }
            public object organizationTaxId { get; set; }
            public object organizationStreet1 { get; set; }
            public object organizationStreet2 { get; set; }
            public object organizationCity { get; set; }
            public object organizationStateId { get; set; }
            public int organizationCountryId { get; set; }
            public string organizationZipCode { get; set; }
            public object organizationBankAccountName { get; set; }
            public object organizationBankAccountField1 { get; set; }
            public object organizationBankAccountField2 { get; set; }
            public string clientFirstName { get; set; }
            public string clientLastName { get; set; }
            public object clientCompanyName { get; set; }
            public object clientCompanyRegistrationNumber { get; set; }
            public object clientCompanyTaxId { get; set; }
            public string clientStreet1 { get; set; }
            public string clientStreet2 { get; set; }
            public string clientCity { get; set; }
            public int clientCountryId { get; set; }
            public object clientStateId { get; set; }
            public string clientZipCode { get; set; }
            public List<object> attributes { get; set; }
            public bool uncollectible { get; set; }
            public bool proforma { get; set; }
            public object generatedInvoiceId { get; set; }
            public object proformaInvoiceId { get; set; }
            public bool isAppliedVatReverseCharge { get; set; }
        }
    }
}
