using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace PortalPagos.Models
{
    public class HttpHandler
    {
        string method = "";
        string authKey = "";
        string url = "";
        string shortener = "";
        string json = "";
        public HttpHandler(string method, string authKey, string url, string shortener, string json)
        {
            this.method = method;
            this.authKey = authKey;
            this.url = url;
            this.shortener = shortener;
            this.json = json;
        }

        public async Task<String> doRequest()
        {
            using (var client = new HttpClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                if (method == "GET")
                {
                    //GET
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("X-Auth-App-Key", authKey);
                    //                client.DefaultRequestHeaders.Authorization =
                    //new AuthenticationHeaderValue("X-Auth-App-Key", "Qygxrlhlu9VvqOssEjJXW+M7MoCQcasxMC6X7wf/JFDJke1VOidFhRxJQ7GU44Bq");
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(shortener);
                    if (response.IsSuccessStatusCode)
                    {
                        var s = await response.Content.ReadAsStringAsync();
                        return s;

                    }
                    else
                        return "NOK";
                }
                else
                {
                    //POST
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add("X-Auth-App-Key", authKey);
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != "")
                            return result;
                        else
                            return "NOK";

                    }



                }

            }

        
        }


        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}