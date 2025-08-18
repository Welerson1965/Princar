namespace MercadoLivre.Seguranca.Domain.DTOs
{
    public class NotaFiscalRetornoDTO
    {
        public long id { get; set; }
        public string status { get; set; }
        public string transaction_status { get; set; }
        public Issuer issuer { get; set; }
        public Recipient recipient { get; set; }
        public Shipment shipment { get; set; }
        public Item[] items { get; set; }
        public DateTime issued_date { get; set; }
        public string invoice_series { get; set; }
        public int invoice_number { get; set; }
        public Attributes attributes { get; set; }
        public Fiscal_Data fiscal_data { get; set; }
        public float amount { get; set; }
        public float items_amount { get; set; }
        public object[] errors { get; set; }
        public int items_quantity { get; set; }
        public object pack_id { get; set; }
        public object custom_issuer_address { get; set; }
        public string site_id { get; set; }
        public object other_amount { get; set; }

        public class Issuer
        {
            public string user_id { get; set; }
            public object brand_name { get; set; }
            public string name { get; set; }
            public Phone phone { get; set; }
            public Address address { get; set; }
            public Identifications identifications { get; set; }
        }

        public class Phone
        {
            public string area_code { get; set; }
            public string number { get; set; }
        }

        public class Address
        {
            public string street_name { get; set; }
            public string street_number { get; set; }
            public string complement { get; set; }
            public string neighborhood { get; set; }
            public string city { get; set; }
            public string zip_code { get; set; }
            public string state { get; set; }
            public string country { get; set; }
        }

        public class Identifications
        {
            public string cnpj { get; set; }
            public string crt { get; set; }
            public string ie { get; set; }
            public string ie_type { get; set; }
        }

        public class Recipient
        {
            public string external_recipient_id { get; set; }
            public string name { get; set; }
            public object phone { get; set; }
            public Address1 address { get; set; }
            public Identifications1 identifications { get; set; }
        }

        public class Address1
        {
            public string street_name { get; set; }
            public string street_number { get; set; }
            public string complement { get; set; }
            public string neighborhood { get; set; }
            public string city { get; set; }
            public string zip_code { get; set; }
            public string state { get; set; }
            public string country { get; set; }
        }

        public class Identifications1
        {
            public string cpf { get; set; }
            public string cnpj { get; set; }
            public object crt { get; set; }
            public string ie { get; set; }
            public string ie_type { get; set; }
        }

        public class Shipment
        {
            public long id { get; set; }
            public string site_id { get; set; }
            public string mode { get; set; }
            public string logistic_type { get; set; }
            public string buyer_cost { get; set; }
            public string paid_by { get; set; }
            public Carrier carrier { get; set; }
            public Volume[] volumes { get; set; }
            public string fiscal_model_id { get; set; }
            public object[] shipping_locations { get; set; }
        }

        public class Carrier
        {
            public string name { get; set; }
            public object phone { get; set; }
            public Address2 address { get; set; }
            public Identifications2 identifications { get; set; }
        }

        public class Address2
        {
            public string street_name { get; set; }
            public string street_number { get; set; }
            public object complement { get; set; }
            public string neighborhood { get; set; }
            public string city { get; set; }
            public object zip_code { get; set; }
            public string state { get; set; }
            public string country { get; set; }
        }

        public class Identifications2
        {
            public string cnpj { get; set; }
            public object crt { get; set; }
            public string ie { get; set; }
            public string ie_type { get; set; }
        }

        public class Volume
        {
            public float net_weight { get; set; }
            public float gross_weight { get; set; }
        }

        public class Attributes
        {
            public string order_source { get; set; }
            public string invoice_source { get; set; }
            public string invoice_key { get; set; }
            public string environment_type { get; set; }
            public string xml_version { get; set; }
            public int status_code { get; set; }
            public string status_description { get; set; }
            public string receipt { get; set; }
            public DateTime? receipt_date { get; set; }
            public DateTime? invoice_creation_date { get; set; }
            public string protocol { get; set; }
            public object invoice_type { get; set; }
            public string emission_type { get; set; }
            public DateTime authorization_date { get; set; }
            public object cancellation_protocol { get; set; }
            public object cancellation_date { get; set; }
            public object cancellation_reason { get; set; }
            public object cancellation_error_code { get; set; }
            public object cancellation_error_description { get; set; }
            public string danfe { get; set; }
            public string document { get; set; }
            public string cnf { get; set; }
            public object correction_letter { get; set; }
            public object reference_invoice { get; set; }
            public Reference_Invoices[] reference_invoices { get; set; }
            public string danfe_location { get; set; }
            public string xml_location { get; set; }
            public object include_freight { get; set; }
            public Third_Party_Authorizations[] third_party_authorizations { get; set; }
        }

        public class Reference_Invoices
        {
            public long id { get; set; }
            public string invoice_key { get; set; }
        }

        public class Third_Party_Authorizations
        {
            public Identifications3 identifications { get; set; }
        }

        public class Identifications3
        {
            public string cpf { get; set; }
        }

        public class Fiscal_Data
        {
            public string customer_type { get; set; }
            public string transaction_type { get; set; }
            public string transaction_type_description { get; set; }
            public Message[] messages { get; set; }
            public Fiscal_Amounts[] fiscal_amounts { get; set; }
            public string state_calculation_type { get; set; }
        }

        public class Message
        {
            public string type { get; set; }
            public string content { get; set; }
        }

        public class Fiscal_Amounts
        {
            public string name { get; set; }
            public Attributes1 attributes { get; set; }
        }

        public class Attributes1
        {
            public int vpis { get; set; }
            public int vbcst { get; set; }
            public int vst { get; set; }
            public int vicms { get; set; }
            public int vbc { get; set; }
            public int vicmsdeson { get; set; }
            public float vtottrib { get; set; }
            public int vcofins { get; set; }
            public float amount { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public string invoice_id { get; set; }
            public string seller_id { get; set; }
            public string pack_id { get; set; }
            public string external_order_id { get; set; }
            public string external_product_id { get; set; }
            public string external_variant_id { get; set; }
            public Attributes2 attributes { get; set; }
            public string product_name { get; set; }
            public int quantity { get; set; }
            public float total_amount { get; set; }
            public float shipping_buyer_cost { get; set; }
            public Discount_Amount discount_amount { get; set; }
            public Fiscal_Data1 fiscal_data { get; set; }
            public object[] payments { get; set; }
            public object additional_info { get; set; }
            public int other_amount { get; set; }
        }

        public class Attributes2
        {
            public string ean { get; set; }
            public string sku { get; set; }
            public string type { get; set; }
            public int bundle_quantity { get; set; }
        }

        public class Discount_Amount
        {
            public object unconditional { get; set; }
            public object conditional { get; set; }
        }

        public class Fiscal_Data1
        {
            public Attributes3 attributes { get; set; }
            public Message1[] messages { get; set; }
            public Rule[] rules { get; set; }
        }

        public class Attributes3
        {
            public string ncm { get; set; }
            public string cest { get; set; }
            public object tax_rule_id { get; set; }
            public string origin_type { get; set; }
            public string origin_detail { get; set; }
            public string cfop { get; set; }
            public string measurement_unit { get; set; }
            public object fci { get; set; }
            public object extipi { get; set; }
            public string csosn { get; set; }
            public object reduction_bc_composition { get; set; }
        }

        public class Message1
        {
            public string type { get; set; }
            public string content { get; set; }
        }

        public class Rule
        {
            public string name { get; set; }
            public Attributes4 attributes { get; set; }
        }

        public class Attributes4
        {
            public string csosn { get; set; }
            public string cst { get; set; }
            public float value { get; set; }
            public float municipal_tax { get; set; }
            public float vibpt { get; set; }
            public float pibpt { get; set; }
            public float federal_national_tax { get; set; }
            public Message2[] messages { get; set; }
            public float federal_imported_tax { get; set; }
            public float state_tax { get; set; }
        }

        public class Message2
        {
            public string type { get; set; }
            public string value { get; set; }
        }
    }
}

