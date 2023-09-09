using PortalPagos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace PortalPagos.Controllers.Cliente
{
    public class FacturacionController : Controller
    {
        
        // GET: Facturacion
        public ActionResult Facturacion()
        {
            if (Session != null && Session["Session"] != null && Session["Session"].ToString() == "session_created")
            {
                using (RedZEntities db = new RedZEntities())
                {
                    var clientId = Convert.ToInt32(Session["clientId"].ToString());
                    var datos_fiscales = db.DatosFiscales.Where(a => a.id_cliente == clientId).FirstOrDefault();
                    if (datos_fiscales != null)
                    {
                        Session["datos_fiscales"] = datos_fiscales;
                        Session["flag_datos"] = true;
                    }
                    else
                        Session["flag_datos"] = false;
                }
                return View(ViewBag);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public JsonResult GetDatosFiscales()
        {
            var __h = Session["datos_fiscales"];
            DatosFiscales df = (DatosFiscales)__h;
            return Json(df);
        }

        public JsonResult SaveDatosFiscales(string form,string action)
        {
            try
            {
               
                var datos_fiscales = System.Web.Helpers.Json.Decode(form);
                using (RedZEntities db = new RedZEntities())
                {
                    if (action == "edit")
                    {
                        var clientId = (int)Session["clientId"];
                        var df = db.DatosFiscales.Where(a => a.id_cliente == clientId).FirstOrDefault();
                        df.id_cliente = Convert.ToInt32(Session["clientId"].ToString());
                        df.nombre = datos_fiscales[0].value;
                        df.rfc = datos_fiscales[1].value;
                        df.uso_cfdi = datos_fiscales[2].value;
                        df.regimen_fiscal = datos_fiscales[3].value;
                        df.direccion = datos_fiscales[4].value; ;
                        df.colonia = datos_fiscales[9].value;
                        df.pais = datos_fiscales[5].value;
                        if (datos_fiscales[5].value == "MX")
                            df.estado = datos_fiscales[6].value;
                        else if (datos_fiscales[5].value == "USA")
                            df.estado = datos_fiscales[7].value;
                        else if (datos_fiscales[5].value == "CAN")
                            df.estado = datos_fiscales[8].value;
                        df.codigo_postal = datos_fiscales[10].value;
                        df.fecha = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        //save
                        DatosFiscales df = new DatosFiscales();
                        df.id_cliente = Convert.ToInt32(Session["clientId"].ToString());
                        df.nombre = datos_fiscales[0].value;
                        df.rfc = datos_fiscales[1].value;
                        df.uso_cfdi = datos_fiscales[2].value;
                        df.regimen_fiscal = datos_fiscales[3].value;
                        df.direccion = datos_fiscales[4].value; ;
                        df.colonia = datos_fiscales[9].value;
                        df.pais = datos_fiscales[5].value;
                        if (datos_fiscales[5].value == "MX")
                            df.estado = datos_fiscales[6].value;
                        else if (datos_fiscales[5].value == "USA")
                            df.estado = datos_fiscales[7].value;
                        else if (datos_fiscales[5].value == "CAN")
                            df.estado = datos_fiscales[8].value;
                        df.codigo_postal = datos_fiscales[10].value;
                        df.fecha = DateTime.Now;
                        db.DatosFiscales.Add(df);
                        db.SaveChanges();
                    }
                  
                    return Json("OK");
                }
            }
            catch (Exception ex)
            {

                return Json("Error : "+ ex.Message);
            }
            
         
        }

        public JsonResult GetFacturasSinTimbrar()
        {
            using (RedZEntities db = new RedZEntities())
            {
                var clientId = (int)Session["clientId"];
                var facturas = db.PagosSinTimbrar.Where(a => a.id_cliente == clientId).ToList();
                return Json(facturas);
            }
            
        }

        public JsonResult SetNuevoArchivoTimbre(string invoice, int id_invoice)
        {
            try
            {
                var create = @"C:\CREATE_Timbrado_40";
                var poll = @"C:\POLL_Timbrado_40";
                var file = Path.Combine(create, "I" + invoice + ".txt");
                var file_poll = Path.Combine(poll, "I" + invoice + ".txt");
                if (!Directory.Exists(file))
                {

                    using (StreamWriter sw = System.IO.File.CreateText(file))
                    {
                        using (RedZEntities db = new RedZEntities())
                        {
                            var d = db.DatosFacturacion.Where(a => a.id_invoice == id_invoice).FirstOrDefault();
                            sw.WriteLine("E;cfdi:Comprobante= ");
                            sw.WriteLine("A;xmlns:cfdi=http://www.sat.gob.mx/cfd/4");
                            sw.WriteLine("A;xmlns:xsi=http://www.w3.org/2001/XMLSchema-instance");
                            sw.WriteLine("A;xsi:schemaLocation=http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd");
                            sw.WriteLine("A;Version=4.0");
                            sw.WriteLine("A;Serie=I");
                            sw.WriteLine("A;Folio=" + d.invoice);
                            sw.WriteLine("A;Fecha=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                            sw.WriteLine("A;NoCertificado=[AUTO]");
                            sw.WriteLine("A;FormaPago=03");
                            sw.WriteLine("A;CondicionesDePago=999 DIAS");
                            sw.WriteLine("A;SubTotal=" + d.monto);
                            sw.WriteLine("A;Descuento=0.00");
                            sw.WriteLine("A;Moneda=MXN");
                            sw.WriteLine("A;Total=" + (d.monto + d.monto_iva).ToString());
                            sw.WriteLine("A;TipoDeComprobante=I");
                            sw.WriteLine("A;MetodoPago=PUE");
                            sw.WriteLine("A;Exportacion=01");
                            sw.WriteLine("A;LugarExpedicion=49000");
                            sw.WriteLine("E;cfdi:InformacionGlobal=");
                            sw.WriteLine("A;Periodicidad=");
                            sw.WriteLine("A;Meses=");
                            sw.WriteLine("A;Año=");
                            sw.WriteLine(@"I;\=");
                            sw.WriteLine("E;cfdi:Emisor=");
                            sw.WriteLine("A;Rfc=RZE4900NE3HA3");
                            sw.WriteLine("A;Nombre=REDZ ENTERPRISES");
                            sw.WriteLine("A;RegimenFiscal=601");
                            sw.WriteLine("A;FacAtrAdquirente=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;cfdi:Receptor=");
                            sw.WriteLine("A;Rfc=" + d.rfc);
                            sw.WriteLine("A;Nombre=" + d.nombre);
                            sw.WriteLine("A;RegimenFiscalReceptor=" + d.regimen_fiscal);
                            sw.WriteLine("A;UsoCFDI=" + d.uso_cfdi);
                            sw.WriteLine("A;DomicilioFiscalReceptor=" + d.codigo_postal);
                            sw.WriteLine(@"I;\=");
                            sw.WriteLine("E;cfdi:Conceptos=");
                            sw.WriteLine("E;cfdi:Concepto=");
                            sw.WriteLine("A;ClaveProdServ=81112100");
                            sw.WriteLine("A;NoIdentificacion=1");
                            sw.WriteLine("A;Cantidad=1");
                            sw.WriteLine("A;ClaveUnidad=A9");
                            sw.WriteLine("A;Unidad=");
                            sw.WriteLine("A;Descripcion=Pago del servicio internet " + d.fecha);
                            sw.WriteLine("A;ValorUnitario=" + d.monto);
                            sw.WriteLine("A;Importe=" + d.monto);
                            sw.WriteLine("A;Descuento=0.00");
                            sw.WriteLine("A;ObjetoImp=02");
                            sw.WriteLine("E;cfdi:Impuestos=");
                            sw.WriteLine("E;cfdi:Traslados=");
                            sw.WriteLine("E;cfdi:Traslado=");
                            sw.WriteLine("A;Base=" + d.monto);
                            sw.WriteLine("A;Impuesto=002");
                            sw.WriteLine("A;TipoFactor=Tasa");
                            sw.WriteLine("A;TasaOCuota=0.160000");
                            sw.WriteLine("A;Importe=" + d.monto_iva);
                            sw.WriteLine("I;.=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;cfdi:Impuestos=");
                            sw.WriteLine("A;TotalImpuestosTrasladados=48");
                            sw.WriteLine("E;cfdi:Traslados=");
                            sw.WriteLine("E;cfdi:Traslado=");
                            sw.WriteLine("A;Impuesto=002");
                            sw.WriteLine("A;TipoFactor=Tasa");
                            sw.WriteLine("A;TasaOCuota=0.160000");
                            sw.WriteLine("A;Base=" + d.monto);
                            sw.WriteLine("A;Importe=" + d.monto_iva);
                            sw.WriteLine("I;.=");
                            sw.WriteLine(@"I;\=");
                            sw.WriteLine("E;cfdi:Addenda=");
                            sw.WriteLine("E;CadenaOriginal=");
                            sw.WriteLine("A;co=");
                            sw.WriteLine("I;.= ");
                            sw.WriteLine("E;DatosXII=");
                            sw.WriteLine("A;cfdi=SI");
                            sw.WriteLine("A;docto=FACTURA");
                            sw.WriteLine("A;estatus=1");
                            sw.WriteLine(@"A;impresora=");
                            sw.WriteLine("A;numeroDeImpresiones=2");
                            sw.WriteLine("A;correoDestino=ramon.red@redz.com");
                            sw.WriteLine("A;ivaMXN=0.00");
                            sw.WriteLine("A;totalMXN=" + d.monto_iva);
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;Otros=");
                            sw.WriteLine("A;Agente=INX");
                            sw.WriteLine("A;PedidoInterno=" + d.invoice);
                            sw.WriteLine("A;moneda=$ PESOS");
                            sw.WriteLine("A;viaEmbarque=");
                            sw.WriteLine("A;PedidoYale=");
                            sw.WriteLine("A;Idioma=ESP");
                            sw.WriteLine("A;FolioInterno=" + d.invoice);
                            sw.WriteLine("A;noCliente=");
                            sw.WriteLine("A;branch=");
                            sw.WriteLine("A;ordenVendedor=");
                            sw.WriteLine("A;Contacto=");
                            sw.WriteLine("A;Telefono=");
                            sw.WriteLine("A;TipoCambio=1.0000");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;LugarEntrega=");
                            sw.WriteLine("A;linea1=" + d.direccion);
                            sw.WriteLine("A;linea2=" + d.colonia);
                            sw.WriteLine("A;linea3=" + d.estado + " " + d.pais);
                            sw.WriteLine("A;linea6=" + d.codigo_postal + " " + d.rfc);
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;DomEmisor=");
                            sw.WriteLine("A;Calle=HIDALGO 166A");
                            sw.WriteLine("A;Colonia=EL SALTO");
                            sw.WriteLine("A;Municipio=ZAPOTLAN EL GRANDE");
                            sw.WriteLine("A;Estado=JALISCO");
                            sw.WriteLine("A;Pais=MÉXICO");
                            sw.WriteLine("A;CodigoPostal=49000");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;DomCliente=");
                            sw.WriteLine("A;Calle=" + d.direccion);
                            sw.WriteLine("A;Colonia=" + d.colonia);
                            sw.WriteLine("A;Municipio=");
                            sw.WriteLine("A;Estado=" + d.estado);
                            sw.WriteLine("A;Pais=" + d.pais);
                            sw.WriteLine("A;CodigoPostal=" + d.codigo_postal);
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;Totales=");
                            sw.WriteLine("A;tipoMoneda=MXN");
                            sw.WriteLine("A;subTotal=" + d.monto);
                            sw.WriteLine("A;SubtotalAntesImpuestos=" + d.monto);
                            sw.WriteLine("A;iva=" + d.monto_iva);
                            sw.WriteLine("A;tasaIva=16.00 %");
                            sw.WriteLine("A;total=" + (d.monto + d.monto_iva).ToString());
                            sw.WriteLine("A;totalLetra=");
                            sw.WriteLine("A;TotalPromociones=0");
                            sw.WriteLine("A;TotalDescuentos=0");
                            sw.WriteLine("A;TotalCantidad=1");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;ComentariosGlobales=");
                            sw.WriteLine("E;Comentario=");
                            sw.WriteLine("A;Linea=EXW");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;Comentario=");
                            sw.WriteLine("A;Linea=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;Comentario=");
                            sw.WriteLine("A;Linea=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("I;.=");
                            sw.WriteLine("E;Detalle=");
                            sw.WriteLine("E;Partida=");
                            sw.WriteLine("A;linea=1");
                            sw.WriteLine("A;ClaveProdServ=81112100");
                            sw.WriteLine("A;StockDescription=Pago del servicio internet" + d.fecha);
                            sw.WriteLine("A;OrderUom=Cantidad");
                            sw.WriteLine("A;Price=" + d.monto);
                            sw.WriteLine("A;Promocion=0.00");
                            sw.WriteLine("A;Total=" + (d.monto + d.monto_iva).ToString());
                            sw.WriteLine("A;PaisOrigen=MEX");
                            sw.WriteLine("I;.=");
                        }
                        System.IO.File.Copy(file, file_poll);


                    }
                    return Json("OK");
                }else
                    return Json("NOK");
            }
            catch (Exception ex)
            {

                return Json("Error: "+ ex.Message);
            }
            
            
           
        }
    }
}