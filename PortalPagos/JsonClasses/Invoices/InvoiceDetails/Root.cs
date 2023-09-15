using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalPagos.JsonClasses.Invoices.InvoiceDetails
{
    public class Root
    {
        public int id { get; set; }
        public int clientId { get; set; }
        public string number { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? dueDate { get; set; }
        public DateTime? emailSentDate { get; set; }
        public int maturityDays { get; set; }
        public DateTime? taxableSupplyDate { get; set; }
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
        public List<object> paymentCovers { get; set; }
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