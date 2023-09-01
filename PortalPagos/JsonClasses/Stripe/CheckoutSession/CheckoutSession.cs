using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalPagos.JsonClasses.Stripe.CheckoutSession
{
    public class CheckoutSession
    {
        public class Address
        {
            public object city { get; set; }
            public string country { get; set; }
            public object line1 { get; set; }
            public object line2 { get; set; }
            public object postal_code { get; set; }
            public object state { get; set; }
        }

        public class AutomaticTax
        {
            public bool enabled { get; set; }
            public object status { get; set; }
        }

        public class CustomerDetails
        {
            public Address address { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public object phone { get; set; }
            public string tax_exempt { get; set; }
            public List<object> tax_ids { get; set; }
        }

        public class CustomText
        {
            public object shipping_address { get; set; }
            public object submit { get; set; }
        }

        public class InvoiceCreation
        {
            public bool enabled { get; set; }
            public InvoiceData invoice_data { get; set; }
        }

        public class InvoiceData
        {
            public object account_tax_ids { get; set; }
            public object custom_fields { get; set; }
            public object description { get; set; }
            public object footer { get; set; }
            public Metadata metadata { get; set; }
            public object rendering_options { get; set; }
        }

        public class Metadata
        {
        }

        public class PaymentMethodOptions
        {
            public object acss_debit { get; set; }
            public object affirm { get; set; }
            public object afterpay_clearpay { get; set; }
            public object alipay { get; set; }
            public object au_becs_debit { get; set; }
            public object bacs_debit { get; set; }
            public object bancontact { get; set; }
            public object boleto { get; set; }
            public object card { get; set; }
            public object cashapp { get; set; }
            public object customer_balance { get; set; }
            public object eps { get; set; }
            public object fpx { get; set; }
            public object giropay { get; set; }
            public object grabpay { get; set; }
            public object ideal { get; set; }
            public object klarna { get; set; }
            public object konbini { get; set; }
            public object link { get; set; }
            public object oxxo { get; set; }
            public object p24 { get; set; }
            public object paynow { get; set; }
            public object pix { get; set; }
            public object sepa_debit { get; set; }
            public object sofort { get; set; }
            public object us_bank_account { get; set; }
        }

        public class PhoneNumberCollection
        {
            public bool enabled { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string @object { get; set; }
            public object after_expiration { get; set; }
            public object allow_promotion_codes { get; set; }
            public int amount_subtotal { get; set; }
            public int amount_total { get; set; }
            public AutomaticTax automatic_tax { get; set; }
            public object billing_address_collection { get; set; }
            public string cancel_url { get; set; }
            public object client_reference_id { get; set; }
            public object consent { get; set; }
            public object consent_collection { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public object currency_conversion { get; set; }
            public List<object> custom_fields { get; set; }
            public CustomText custom_text { get; set; }
            public object customer { get; set; }
            public string customer_creation { get; set; }
            public CustomerDetails customer_details { get; set; }
            public object customer_email { get; set; }
            public int expires_at { get; set; }
            public object invoice { get; set; }
            public InvoiceCreation invoice_creation { get; set; }
            public object line_items { get; set; }
            public bool livemode { get; set; }
            public object locale { get; set; }
            public Metadata metadata { get; set; }
            public string mode { get; set; }
            public string payment_intent { get; set; }
            public object payment_link { get; set; }
            public string payment_method_collection { get; set; }
            public PaymentMethodOptions payment_method_options { get; set; }
            public List<string> payment_method_types { get; set; }
            public string payment_status { get; set; }
            public PhoneNumberCollection phone_number_collection { get; set; }
            public object recovered_from { get; set; }
            public object setup_intent { get; set; }
            public object shipping_address_collection { get; set; }
            public object shipping_cost { get; set; }
            public object shipping_details { get; set; }
            public List<object> shipping_options { get; set; }
            public string status { get; set; }
            public object submit_type { get; set; }
            public object subscription { get; set; }
            public string success_url { get; set; }
            public object tax_id_collection { get; set; }
            public TotalDetails total_details { get; set; }
            public object url { get; set; }
        }

        public class TotalDetails
        {
            public int amount_discount { get; set; }
            public int amount_shipping { get; set; }
            public int amount_tax { get; set; }
            public object breakdown { get; set; }
        }
    }
}