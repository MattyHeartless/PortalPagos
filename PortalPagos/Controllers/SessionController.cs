using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json;
using PortalPagos.Models;

namespace PortalPagos.Controllers
{
    public class SessionController : Controller
    {
        // GET: Session
        public async Task<JsonResult> StartSession(string username, string key)
        {
            //if (username == "15975346" && key == "jaja")
            //{

            //    System.Web.HttpContext.Current.Session["Usuario"] = "15975346";
            //    System.Web.HttpContext.Current.Session["Nombre"] = "Pedro Paramo";
            //    System.Web.HttpContext.Current.Session["Session"] = "session_created";
            //    System.Web.HttpContext.Current.Session.Timeout = 99999;
            //    return Json("OK");
            //}
            //else
            //    return Json("Error");
            try
            {
                var json = "{\"username\": \"" + username + "\","
                                  + "\"password\": \"" + key + "\","
                                  + "\"expiration\": 604800,"
                                  + "\"sliding\": 0,"
                                  + "\"deviceName\": \"Webbrowser\"}";
                HttpHandler http = new HttpHandler("POST", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq", 
                    "https://189.199.227.94/crm/api/v1.0/client-zone/login","", json);
                var result = await http.doRequest();
                if (result != "NOK")
                {
                    var login_token = result.Split(':')[1].Replace("\"", "").Replace("\"", "").Replace("}", "");
                    Session["login_token"] = login_token;
                    Session["Session"] = "session_created";
                    Session.Timeout = 99999;
                    var b = await LoadUserInfo(username, key);
                    return Json("OK");
                } else
                    return Json("Error");
            }
            catch (Exception ex)
            {

                return Json("Error: " + ex.Message);
            }
     
          

        }
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session["Session"] = "session_closed";
            return RedirectToAction("Index", "Home");
        }

        public async Task<bool> LoadUserInfo(string username, string key)
        {
            try
            {
                
                var json = "{\"username\": \"" + username + "\","
                                    + "\"password\": \"" + key + "\"}";
                HttpHandler http = new HttpHandler("POST", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq",
                   "https://189.199.227.94/crm/api/v1.0/clients/authenticated", "", json);
                var result = await http.doRequest();
                if (result != "NOK")
                {
                    Root infocliente = JsonConvert.DeserializeObject<Root>(result);
                    Session["clientId"] = infocliente.id;
                    Session["name"] = infocliente.firstName;
                    Session["lastname"] = infocliente.lastName;
                    Session["accountBalance"] = infocliente.accountBalance;
                    Session["address"] = infocliente.fullAddress;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                return false;
            }
            
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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

        public class Root
        {
            public int id { get; set; }
            public string userIdent { get; set; }
            public string previousIsp { get; set; }
            public bool isLead { get; set; }
            public int clientType { get; set; }
            public object companyName { get; set; }
            public object companyRegistrationNumber { get; set; }
            public object companyTaxId { get; set; }
            public object companyWebsite { get; set; }
            public string street1 { get; set; }
            public string street2 { get; set; }
            public string city { get; set; }
            public int countryId { get; set; }
            public object stateId { get; set; }
            public string zipCode { get; set; }
            public string fullAddress { get; set; }
            public object invoiceStreet1 { get; set; }
            public object invoiceStreet2 { get; set; }
            public object invoiceCity { get; set; }
            public object invoiceStateId { get; set; }
            public object invoiceCountryId { get; set; }
            public object invoiceZipCode { get; set; }
            public bool invoiceAddressSameAsContact { get; set; }
            public object note { get; set; }
            public object sendInvoiceByPost { get; set; }
            public object invoiceMaturityDays { get; set; }
            public object stopServiceDue { get; set; }
            public object stopServiceDueDays { get; set; }
            public int organizationId { get; set; }
            public object tax1Id { get; set; }
            public object tax2Id { get; set; }
            public object tax3Id { get; set; }
            public DateTime registrationDate { get; set; }
            public object leadConvertedAt { get; set; }
            public object companyContactFirstName { get; set; }
            public object companyContactLastName { get; set; }
            public bool isActive { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string username { get; set; }
            public List<Contact> contacts { get; set; }
            public List<object> attributes { get; set; }
            public double accountBalance { get; set; }
            public double accountCredit { get; set; }
            public double accountOutstanding { get; set; }
            public string currencyCode { get; set; }
            public string organizationName { get; set; }
            public List<object> bankAccounts { get; set; }
            public List<object> tags { get; set; }
            public DateTime invitationEmailSentDate { get; set; }
            public string avatarColor { get; set; }
            public double addressGpsLat { get; set; }
            public double addressGpsLon { get; set; }
            public bool isArchived { get; set; }
            public object generateProformaInvoices { get; set; }
            public bool usesProforma { get; set; }
            public bool hasOverdueInvoice { get; set; }
            public bool hasOutage { get; set; }
            public bool hasSuspendedService { get; set; }
            public bool hasServiceWithoutDevices { get; set; }
            public object referral { get; set; }
            public bool hasPaymentSubscription { get; set; }
            public bool hasAutopayCreditCard { get; set; }
        }

        public class Type
        {
            public int id { get; set; }
            public string name { get; set; }
        }




    }
}