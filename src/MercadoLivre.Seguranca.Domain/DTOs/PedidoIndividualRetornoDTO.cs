namespace MercadoLivre.DTOs
{
    public class PedidoIndividualRetornoDTO
    {
        public long id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime last_updated { get; set; }
        public DateTime date_closed { get; set; }
        public object pack_id { get; set; }
        public object fulfilled { get; set; }
        public string buying_mode { get; set; }
        public object shipping_cost { get; set; }
        public object[] mediations { get; set; }
        public float total_amount { get; set; }
        public float paid_amount { get; set; }
        public Order_ItemsIndividual[] order_items { get; set; }
        public string currency_id { get; set; }
        public PaymentIndividual[] payments { get; set; }
        public ShippingIndividual shipping { get; set; }
        public string status { get; set; }
        public object status_detail { get; set; }
        public string[] tags { get; set; }
        public string[] internal_tags { get; set; }
        public object[] static_tags { get; set; }
        public FeedbackIndividual feedback { get; set; }
        public ContextIndividual context { get; set; }
        public SellerIndividual seller { get; set; }
        public BuyerIndividual buyer { get; set; }
        public TaxesIndividual taxes { get; set; }
        public object cancel_detail { get; set; }
        public object manufacturing_ending_date { get; set; }
        public Order_RequestIndividual order_request { get; set; }
    }
    public class ShippingIndividual
    {
        public long id { get; set; }
    }

    public class FeedbackIndividual
    {
        public object seller { get; set; }
        public object buyer { get; set; }
    }

    public class ContextIndividual
    {
        public string channel { get; set; }
        public string site { get; set; }
        public object[] flows { get; set; }
    }

    public class SellerIndividual
    {
        public int id { get; set; }
    }

    public class BuyerIndividual
    {
        public int id { get; set; }
        public string nickname { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class TaxesIndividual
    {
        public float? amount { get; set; }
        public object currency_id { get; set; }
        public object id { get; set; }
    }

    public class Order_RequestIndividual
    {
        public object change { get; set; }
        public object _return { get; set; }
    }

    public class Order_ItemsIndividual
    {
        public ItemIndividual item { get; set; }
        public int quantity { get; set; }
        public Requested_QuantityIndividual requested_quantity { get; set; }
        public object picked_quantity { get; set; }
        public float unit_price { get; set; }
        public float full_unit_price { get; set; }
        public string full_unit_price_currency_id { get; set; }
        public string currency_id { get; set; }
        public object manufacturing_days { get; set; }
        public float sale_fee { get; set; }
        public string listing_type_id { get; set; }
        public object base_exchange_rate { get; set; }
        public object base_currency_id { get; set; }
        public object element_id { get; set; }
        public object discounts { get; set; }
        public object bundle { get; set; }
        public object compat_id { get; set; }
        public StockIndividual stock { get; set; }
        public object kit_instance_id { get; set; }
    }

    public class ItemIndividual
    {
        public string id { get; set; }
        public string title { get; set; }
        public string category_id { get; set; }
        public object variation_id { get; set; }
        public object seller_custom_field { get; set; }
        public object[] variation_attributes { get; set; }
        public string warranty { get; set; }
        public string condition { get; set; }
        public string seller_sku { get; set; }
        public object global_price { get; set; }
        public object net_weight { get; set; }
        public string user_product_id { get; set; }
        public object release_date { get; set; }
    }

    public class Requested_QuantityIndividual
    {
        public string measure { get; set; }
        public int value { get; set; }
    }

    public class StockIndividual
    {
        public object store_id { get; set; }
        public string node_id { get; set; }
    }

    public class PaymentIndividual
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public int payer_id { get; set; }
        public CollectorIndividual collector { get; set; }
        public long? card_id { get; set; }
        public string reason { get; set; }
        public string site_id { get; set; }
        public string payment_method_id { get; set; }
        public string currency_id { get; set; }
        public int installments { get; set; }
        public string issuer_id { get; set; }
        public Atm_Transfer_ReferenceIndividual atm_transfer_reference { get; set; }
        public object coupon_id { get; set; }
        public object activation_uri { get; set; }
        public string operation_type { get; set; }
        public string payment_type { get; set; }
        public string[] available_actions { get; set; }
        public string status { get; set; }
        public object status_code { get; set; }
        public string status_detail { get; set; }
        public float? transaction_amount { get; set; }
        public float? transaction_amount_refunded { get; set; }
        public float? taxes_amount { get; set; }
        public float? shipping_cost { get; set; }
        public float? coupon_amount { get; set; }
        public float? overpaid_amount { get; set; }
        public float? total_paid_amount { get; set; }
        public float? installment_amount { get; set; }
        public object deferred_period { get; set; }
        public DateTime date_approved { get; set; }
        public object transaction_order_id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_last_modified { get; set; }
        public float marketplace_fee { get; set; }
        public object reference_id { get; set; }
        public string authorization_code { get; set; }
    }

    public class CollectorIndividual
    {
        public int id { get; set; }
    }

    public class Atm_Transfer_ReferenceIndividual
    {
        public object transaction_id { get; set; }
        public object company_id { get; set; }
    }
}