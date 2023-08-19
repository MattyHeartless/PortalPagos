using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalPagos.JsonClasses.Cliente.InfoCliente
{
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
        public object invitationEmailSentDate { get; set; }
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
}