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
using PortalPagos.JsonClasses.Cliente.InfoCliente;
using System.Globalization;
using PortalPagos.Models;
using Stripe;
using Stripe.Checkout;
using PortalPagos.JsonClasses.Stripe.CheckoutSession;
using System.Threading;

namespace PortalPagos.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public async Task<ActionResult> Dashboard()
        {
            if (Session != null && Session["Session"] != null && Session["Session"].ToString() == "session_created")
            {
                HttpHandler d = new HttpHandler("GET", Session["login_token"].ToString(), "https://189.199.227.94/crm/api/v1.0/", "client-zone/dashboard", "");
                var response = await d.doRequest();
                ClientDashboard c = new ClientDashboard();
                c = JsonConvert.DeserializeObject<ClientDashboard>(response.ToString());
                Session["nextInvoicingDay"] = c.nextInvoicingDay;
                Session["nextInvoicePrice"] = c.nextInvoicePrice;


                HttpHandler http = new HttpHandler("GET", Session["login_token"].ToString(), "https://189.199.227.94/crm/api/v1.0/", "client-zone/services", "");
                var resp = await http.doRequest();
                List<ClientService> cs = new List<ClientService>();
                cs = JsonConvert.DeserializeObject<List<ClientService>>(resp);
                Session["status"] = cs[0].status;
                Session["service_name"] = cs[0].name;
                Session["downloadSpeed"] = cs[0].downloadSpeed;
                Session["uploadSpeed"] = cs[0].uploadSpeed;
                using (RedZEntities db = new RedZEntities())
                {
                    var id = Convert.ToInt32(Session["clientId"].ToString());
                    var usuario = db.StripeCustomer.Where(a => a.id_cliente == id).FirstOrDefault();
                    
                    Task.Run(() => SaveVouchersPaid(usuario.stripe_idcliente, id));
                    
                }
       

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public class ClientDashboard
        {
            public string currencyCode { get; set; }
            public double accountBalance { get; set; }
            public DateTime nextInvoicingDay { get; set; }
            public double nextInvoicePrice { get; set; }
        }

        public class ClientService
        {
            public int id { get; set; }
            public bool prepaid { get; set; }
            public int status { get; set; }
            public string name { get; set; }
            public string fullAddress { get; set; }
            public string street1 { get; set; }
            public string street2 { get; set; }
            public string city { get; set; }
            public int countryId { get; set; }
            public object stateId { get; set; }
            public string zipCode { get; set; }
            public double addressGpsLat { get; set; }
            public double addressGpsLon { get; set; }
            public int servicePlanId { get; set; }
            public int servicePlanPeriodId { get; set; }
            public double price { get; set; }
            public bool hasIndividualPrice { get; set; }
            public double totalPrice { get; set; }
            public string currencyCode { get; set; }
            public object invoiceLabel { get; set; }
            public object contractId { get; set; }
            public int contractLengthType { get; set; }
            public object minimumContractLengthMonths { get; set; }
            public DateTime activeFrom { get; set; }
            public object activeTo { get; set; }
            public object contractEndDate { get; set; }
            public int discountType { get; set; }
            public object discountValue { get; set; }
            public string discountInvoiceLabel { get; set; }
            public object discountFrom { get; set; }
            public object discountTo { get; set; }
            public object tax1Id { get; set; }
            public object tax2Id { get; set; }
            public object tax3Id { get; set; }
            public DateTime invoicingStart { get; set; }
            public int invoicingPeriodType { get; set; }
            public int invoicingPeriodStartDay { get; set; }
            public int nextInvoicingDayAdjustment { get; set; }
            public string servicePlanName { get; set; }
            public double servicePlanPrice { get; set; }
            public int servicePlanPeriod { get; set; }
            public double downloadSpeed { get; set; }
            public double uploadSpeed { get; set; }
            public DateTime lastInvoicedDate { get; set; }
            public object suspensionReasonId { get; set; }
        }


        public async Task<JsonResult> test()
        {
            StripeConfiguration.ApiKey = "sk_test_51Nfr2oLyLqVQHv3YkUaVX2EfN99WvsqVRayeHhWxVKcK8fDCBZwJc60M2TKpNY9OLLxqyj6e0hpszFEIBbtdl6mE00LSbYYMb8";
            var service = new PaymentIntentService();
            var response = service.Get("pi_3NlbrdLyLqVQHv3Y16snZ5tU");
            
            return Json("");
        }


        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);


        public JsonResult GetVouchers()
        {
            StripeConfiguration.ApiKey = "sk_test_51Nfr2oLyLqVQHv3YkUaVX2EfN99WvsqVRayeHhWxVKcK8fDCBZwJc60M2TKpNY9OLLxqyj6e0hpszFEIBbtdl6mE00LSbYYMb8";
            Dictionary<string, Dictionary<string, string>> s = new Dictionary<string, Dictionary<string, string>>();

            using (RedZEntities db = new RedZEntities())
            {
                var id = Convert.ToInt32(Session["clientId"].ToString());
                var usuario = db.StripeCustomer.Where(a => a.id_cliente == id).FirstOrDefault();
                if (usuario != null)
                {
                    var options = new PaymentIntentListOptions
                    {
                        Customer = usuario.stripe_idcliente,
                    };
                    var service = new PaymentIntentService();
                    StripeList<PaymentIntent> paymentIntents = service.List(
                      options);
                    var oxxo = paymentIntents.Where(a => a.PaymentMethodTypes[0] == "oxxo").Select(b=> new { b.Amount, b.Status, b.NextAction, b.Created, b.Metadata}).ToList();
                    
                    return Json(oxxo);
                }
                else
                    return Json("NOK");

            }

        }
        public async void SaveVouchersPaid(string StripeUser, int clientId)
        {
            using (RedZEntities db = new RedZEntities())
            {
                StripeConfiguration.ApiKey = "sk_test_51Nfr2oLyLqVQHv3YkUaVX2EfN99WvsqVRayeHhWxVKcK8fDCBZwJc60M2TKpNY9OLLxqyj6e0hpszFEIBbtdl6mE00LSbYYMb8";
                var options = new PaymentIntentListOptions
                {
                    Customer = StripeUser,
                };
                var service = new PaymentIntentService();
                StripeList<PaymentIntent> paymentIntents = service.List(
                  options);
                var oxxo = paymentIntents.Where(a => a.PaymentMethodTypes[0] == "oxxo").ToList();
                foreach (var item in oxxo)
                {
                    
                    if (item.Metadata.ContainsKey("invoice_id") && item.Status == "succeeded")
                    {
                        var invoiceId = Convert.ToInt32(item.Metadata["invoice_id"]);
                        var voucher = db.Pagos.Where(a => a.id_invoice == invoiceId).FirstOrDefault();
                        if(voucher == null)
                        {
                            Pagos p = new Pagos();
                            p.id_cliente = clientId;
                            p.id_invoice = Convert.ToInt32(invoiceId);
                            p.monto = Convert.ToDecimal(item.Amount);
                            p.fecha_voucher = DateTime.Now;
                            p.tipo = "OXXO";
                            db.Pagos.Add(p);
                            db.SaveChanges();
                            var ammount = await LoadInvoiceQty(invoiceId);
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://189.199.227.94/crm/api/v1.0/payments");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Headers.Add("X-Auth-App-Key", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq");
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                //string json = "{\"user\":\"test\"," +
                                //              "\"password\":\"bla\"}";
                                //Json POST hacia el CRM 
                                var jsonn = "{\"currencyCode\": \"MXN\",\"applyToInvoicesAutomatically\": true,"
                                             + "\"invoiceIds\": [" + invoiceId + "],"
                                             + "\"clientId\": " + clientId.ToString() + ","
                                             + "\"methodId\": \"1dd098fa-5d63-4c8d-88b7-3c27ffbbb6ae\","
                                             + "\"checkNumber\": \"\","
                                             + "\"createdDate\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "+000\","
                                             + "\"amount\": " + ammount + ","
                                             + "\"note\": \"Pago del mes servicio Internet\","
                                             + "\"providerPaymentTime\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "+000\"}";
                                streamWriter.Write(jsonn);
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            
        }




        public async Task<double> LoadInvoiceQty(int invoiceId)
        {
            using (var clients = new HttpClient())
            {
                HttpHandler http = new HttpHandler("GET", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq", "https://189.199.227.94/crm/api/v1.0/", "invoices/" + invoiceId, "");
                var resp = await http.doRequest();
                PortalPagos.JsonClasses.Invoices.InvoiceDetails.Root invoice = new PortalPagos.JsonClasses.Invoices.InvoiceDetails.Root();
                invoice = JsonConvert.DeserializeObject<PortalPagos.JsonClasses.Invoices.InvoiceDetails.Root>(resp);
                return invoice.amountToPay; 





            }
        }


        public ActionResult Pagos()
        {
            if (Session != null && Session["Session"] != null && Session["Session"].ToString() == "session_created")
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Vouchers()
        {
            if (Session != null && Session["Session"] != null && Session["Session"].ToString() == "session_created")
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public void testApi()
        {

        }

        public async Task<ActionResult> success()
        {
            using (var clients = new HttpClient())
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://189.199.227.94/crm/api/v1.0/payments");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("X-Auth-App-Key", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq");
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = "{\"user\":\"test\"," +
                    //              "\"password\":\"bla\"}";
                    //Json POST hacia el CRM 
                    var jsonn = "{\"currencyCode\": \"MXN\",\"applyToInvoicesAutomatically\": true,"
                                 + "\"invoiceIds\": [" + Session["invoiceId"].ToString() + "],"
                                 + "\"clientId\": " + Session["clientId"].ToString() + ","
                                 + "\"methodId\": \"1dd098fa-5d63-4c8d-88b7-3c27ffbbb6ae\","
                                 + "\"checkNumber\": \"\","
                                 + "\"createdDate\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "+000\","
                                 + "\"amount\": " + Session["ammountToPay"].ToString() + ","
                                 + "\"note\": \"Pago del mes servicio Internet\","
                                 + "\"providerPaymentTime\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "+000\"}";
                    streamWriter.Write(jsonn);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                StripeConfiguration.ApiKey = "sk_test_51Nfr2oLyLqVQHv3YkUaVX2EfN99WvsqVRayeHhWxVKcK8fDCBZwJc60M2TKpNY9OLLxqyj6e0hpszFEIBbtdl6mE00LSbYYMb8";
                Dictionary<string, Dictionary<string, string>> s = new Dictionary<string, Dictionary<string, string>>();

                //var service = new SessionService();
                //var session = service.Get();



                //var options = new PaymentIntentListOptions
                //{
                //    Customer = "4",
                //};
                //var service = new PaymentIntentService();
                //StripeList<PaymentIntent> paymentIntents = service.List(options);

                //foreach (var item in paymentIntents.Data)
                //{

                //}


                using (RedZEntities db = new RedZEntities())
                {
                    Pagos p = new Pagos();
                    p.id_cliente = Convert.ToInt32( Session["clientId"].ToString());
                    p.id_invoice = Convert.ToInt32(Session["invoiceId"].ToString());
                    p.monto = Convert.ToDecimal( Session["Total"].ToString());
                    p.fecha = DateTime.Now;
                    p.tipo = "Tarjeta";
                    db.Pagos.Add(p);
                    db.SaveChanges();
                }


            }
            return View();
        
        }

        public ActionResult canceled()
        {
            return View();
        }

       

      



    }
}