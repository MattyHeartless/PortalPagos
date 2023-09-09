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
using Stripe;
using Stripe.Checkout;
using PortalPagos.JsonClasses.Invoices.InvoiceDetails;
using PortalPagos.Models;

namespace PortalPagos.Controllers.Cliente
{
    public class PagosController : Controller
    {
        // GET: Pagos
        string methodId = "1dd098fa-5d63-4c8d-88b7-3c27ffbbb6ae";
        public async Task<ActionResult> getPagos()
        {
           
                HttpHandler http = new HttpHandler("GET", Session["login_token"].ToString(), "https://189.199.227.94/crm/api/v1.0/", "client-zone/invoices","");
                var resp = await http.doRequest();
                List<Root> list = new List<Root>();
                list = JsonConvert.DeserializeObject<List<Root>>(resp);

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
                    inv.clientId = Convert.ToInt32( Session["clientId"].ToString());
                    linv.Add(inv);

                }

                return Json(linv);
           

        }
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
      

        public class Invoices
        {
            public int id { get; set; }
            public string number { get; set; }
            public double amountToPay { get; set; }
            public double total { get; set; }
            public int status { get; set; }
            public string type { get; set; }
            public string label { get; set; }
            public int clientId { get; set; }
        }

        public class Item
        {
            public int id { get; set; }
            public string type { get; set; }
            public string label { get; set; }
            public double price { get; set; }
            public double quantity { get; set; }
            public double total { get; set; }
            public string unit { get; set; }
            public object tax1Id { get; set; }
            public object tax2Id { get; set; }
            public object tax3Id { get; set; }
            public int? serviceId { get; set; }
            public object serviceSurchargeId { get; set; }
            public int? productId { get; set; }
            public double? discountPrice { get; set; }
            public double? discountQuantity { get; set; }
            public double? discountTotal { get; set; }
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
            public string number { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime dueDate { get; set; }
            public DateTime? emailSentDate { get; set; }
            public int maturityDays { get; set; }
            public DateTime taxableSupplyDate { get; set; }
            public string notes { get; set; }
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
            public bool proforma { get; set; }
            public object generatedInvoiceId { get; set; }
            public object proformaInvoiceId { get; set; }
            public bool isAppliedVatReverseCharge { get; set; }
        }



        public async Task<ActionResult> CreatePaySession()
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51Nfr2oLyLqVQHv3YkUaVX2EfN99WvsqVRayeHhWxVKcK8fDCBZwJc60M2TKpNY9OLLxqyj6e0hpszFEIBbtdl6mE00LSbYYMb8";

                var domain = "http://localhost:61277/";
                var invoiceId = Request["invoiceId"].ToString();
                var s = await LoadInvoice(Convert.ToInt32(invoiceId));
                //int grupo = Convert.ToInt32(Request["id_grupo"].ToString());
                var Concepto = Request["lblconcepto"].ToString();
                double precio = Convert.ToDouble(Session["Total"].ToString());
                //long s = Convert.long;
                //Session["grupo_pago"] = grupo;
                Session["concepto_pago"] = Concepto;
                string customerId = "";
                using (RedZEntities db = new RedZEntities())
                {
                    var id_cliente = Convert.ToInt32(Session["clientId"].ToString());
                    var clientstripe = db.StripeCustomer.Where(a => a.id_cliente == id_cliente).FirstOrDefault();
                    if (clientstripe != null)
                    {
                        var service = new CustomerService();
                        var resposne = service.Get(clientstripe.stripe_idcliente);
                        customerId = clientstripe.stripe_idcliente;
                    }

                    else
                    {
                        var name = Session["name"].ToString() + " " + Session["lastname"].ToString();
                        var CustomerOptions = new CustomerCreateOptions
                        {
                            Name = name,
                        };
                        var service = new CustomerService();
                        var resposne = service.Create(CustomerOptions);

                        StripeCustomer sc = new StripeCustomer();
                        sc.id_cliente = id_cliente;
                        sc.stripe_idcliente = resposne.Id;
                        db.StripeCustomer.Add(sc);
                        db.SaveChanges();
                        Session["StripeCustomerID"] = resposne.Id;

                        customerId = resposne.Id;

                    }
                }



                var options = new SessionCreateOptions
                {
                    Customer = customerId,
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                        "oxxo",
                    },

                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(precio * 100),
                                Currency = "mxn",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = Concepto,
                                },
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    SuccessUrl = domain + "/Cliente/success",
                    CancelUrl = domain + "/Cliente/Pagos",
                    PaymentIntentData = new SessionPaymentIntentDataOptions
                    {
                        Metadata = new Dictionary<string, string>
                        {
                            {"Customer", Session["clientId"].ToString() },
                            //{"grupo_pago" , grupo.ToString()},
                            {"Concepto",  Concepto},
                            {"invoice_id", invoiceId }
                            
                        }

                    }
                };
                var serviceP = new SessionService();
                Session session = serviceP.Create(options);
                Session["PaySessionId"] = session.Id;
              
           

                //Response.Headers.Add("Location", session.Url);
                var url = session.Url;
                return Redirect(url);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {

                return Redirect("/Pagos/cancel");
            }

        }

        public async Task<bool> LoadInvoice(int invoiceId)
        {
            using (var clients = new HttpClient())
            {
                HttpHandler http = new HttpHandler("GET", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq", "https://189.199.227.94/crm/api/v1.0/", "invoices/" + invoiceId, "");
                var resp = await http.doRequest();
                
            
                PortalPagos.JsonClasses.Invoices.InvoiceDetails.Root invoice = new PortalPagos.JsonClasses.Invoices.InvoiceDetails.Root();    
                    invoice = JsonConvert.DeserializeObject<PortalPagos.JsonClasses.Invoices.InvoiceDetails.Root>(resp);
                    Session["invoiceId"] = invoice.id;
                    Session["ammountToPay"] = invoice.amountToPay;
                    Session["Total"] = invoice.amountToPay + ((invoice.amountToPay * 0.0457) + 3);
                    Session["clientId"] = invoice.clientId;
                    Session["invoice"] = invoice.number;
                    return true;
                
               



            }
        }


        
    }
}
