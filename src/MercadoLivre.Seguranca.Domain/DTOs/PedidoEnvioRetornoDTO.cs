namespace MercadoLivre.Seguranca.Domain.DTOs
{
    public class PedidoEnvioRetornoDTO
    {
        public Substatus_History[] substatus_history { get; set; }
        public Snapshot_Packing snapshot_packing { get; set; }
        public string receiver_id { get; set; }
        public float base_cost { get; set; }
        public Status_History status_history { get; set; }
        public string type { get; set; }
        public object return_details { get; set; }
        public int sender_id { get; set; }
        public string mode { get; set; }
        public float order_cost { get; set; }
        public Priority_Class priority_class { get; set; }
        public int service_id { get; set; }
        public Shipping_Items[] shipping_items { get; set; }
        public string tracking_number { get; set; }
        public Cost_Components cost_components { get; set; }
        public long id { get; set; }
        public string tracking_method { get; set; }
        public DateTime last_updated { get; set; }
        public string[] items_types { get; set; }
        public object comments { get; set; }
        public object substatus { get; set; }
        public DateTime date_created { get; set; }
        public DateTime? date_first_printed { get; set; }
        public string created_by { get; set; }
        public object application_id { get; set; }
        public Shipping_Option shipping_option { get; set; }
        public object[] tags { get; set; }
        public Sender_Address sender_address { get; set; }
        public Sibling sibling { get; set; }
        public object return_tracking_number { get; set; }
        public string site_id { get; set; }
        public object carrier_info { get; set; }
        public string market_place { get; set; }
        public Receiver_Address receiver_address { get; set; }
        public object customer_id { get; set; }
        public long order_id { get; set; }
        public object quotation { get; set; }
        public string status { get; set; }
        public string logistic_type { get; set; }

        public class Snapshot_Packing
        {
            public string snapshot_id { get; set; }
            public string pack_hash { get; set; }
        }

        public class Status_History
        {
            public DateTime? date_shipped { get; set; }
            public DateTime? date_returned { get; set; }
            public DateTime? date_delivered { get; set; }
            public DateTime? date_first_visit { get; set; }
            public DateTime? date_not_delivered { get; set; }
            public DateTime? date_cancelled { get; set; }
            public DateTime? date_handling { get; set; }
            public DateTime? date_ready_to_ship { get; set; }
        }

        public class Priority_Class
        {
            public string id { get; set; }
        }

        public class Cost_Components
        {
            public decimal? loyal_discount { get; set; }
            public int special_discount { get; set; }
            public int compensation { get; set; }
            public float gap_discount { get; set; }
            public float ratio { get; set; }
        }

        public class Shipping_Option
        {
            public object processing_time { get; set; }
            public string cost { get; set; }
            public Estimated_Schedule_Limit estimated_schedule_limit { get; set; }
            public int shipping_method_id { get; set; }
            public Estimated_Delivery_Final estimated_delivery_final { get; set; }
            public Buffering buffering { get; set; }
            public Desired_Promised_Delivery desired_promised_delivery { get; set; }
            public Pickup_Promise pickup_promise { get; set; }
            public float list_cost { get; set; }
            public Estimated_Delivery_Limit estimated_delivery_limit { get; set; }
            public Priority_Class1 priority_class { get; set; }
            public string delivery_promise { get; set; }
            public string delivery_type { get; set; }
            public Estimated_Delivery_Time estimated_delivery_time { get; set; }
            public string name { get; set; }
            public long id { get; set; }
            public Estimated_Delivery_Extended estimated_delivery_extended { get; set; }
            public string currency_id { get; set; }
        }

        public class Estimated_Schedule_Limit
        {
            public object date { get; set; }
        }

        public class Estimated_Delivery_Final
        {
            public DateTime date { get; set; }
        }

        public class Buffering
        {
            public object date { get; set; }
        }

        public class Desired_Promised_Delivery
        {
            public object from { get; set; }
        }

        public class Pickup_Promise
        {
            public object from { get; set; }
            public object to { get; set; }
        }

        public class Estimated_Delivery_Limit
        {
            public DateTime date { get; set; }
        }

        public class Priority_Class1
        {
            public string id { get; set; }
        }

        public class Estimated_Delivery_Time
        {
            public DateTime date { get; set; }
            public DateTime pay_before { get; set; }
            public object schedule { get; set; }
            public string unit { get; set; }
            public Offset offset { get; set; }
            public int shipping { get; set; }
            public Time_Frame time_frame { get; set; }
            public int handling { get; set; }
            public string type { get; set; }
        }

        public class Offset
        {
            public object date { get; set; }
            public object shipping { get; set; }
        }

        public class Time_Frame
        {
            public object from { get; set; }
            public object to { get; set; }
        }

        public class Estimated_Delivery_Extended
        {
            public DateTime date { get; set; }
        }

        public class Sender_Address
        {
            public Country country { get; set; }
            public string address_line { get; set; }
            public string[] types { get; set; }
            public object scoring { get; set; }
            public object agency { get; set; }
            public City city { get; set; }
            public object geolocation_type { get; set; }
            public int latitude { get; set; }
            public Municipality municipality { get; set; }
            public object location_id { get; set; }
            public string street_name { get; set; }
            public string zip_code { get; set; }
            public object geolocation_source { get; set; }
            public Node node { get; set; }
            public object intersection { get; set; }
            public string street_number { get; set; }
            public object comment { get; set; }
            public object id { get; set; }
            public State state { get; set; }
            public Neighborhood neighborhood { get; set; }
            public object geolocation_last_updated { get; set; }
            public int longitude { get; set; }
        }

        public class Country
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Municipality
        {
            public object id { get; set; }
            public object name { get; set; }
        }

        public class Node
        {
            public string logistic_center_id { get; set; }
            public string node_id { get; set; }
        }

        public class State
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Neighborhood
        {
            public object id { get; set; }
            public string name { get; set; }
        }

        public class Sibling
        {
            public object reason { get; set; }
            public object sibling_id { get; set; }
            public object description { get; set; }
            public object source { get; set; }
            public object date_created { get; set; }
            public object last_updated { get; set; }
        }

        public class Receiver_Address
        {
            public Country1 country { get; set; }
            public City1 city { get; set; }
            public string geolocation_type { get; set; }
            public float latitude { get; set; }
            public Municipality1 municipality { get; set; }
            public int? location_id { get; set; }
            public string street_name { get; set; }
            public string zip_code { get; set; }
            public object intersection { get; set; }
            public string receiver_name { get; set; }
            public int? id { get; set; }
            public State1 state { get; set; }
            public float longitude { get; set; }
            public string address_line { get; set; }
            public string[] types { get; set; }
            public string scoring { get; set; }
            public object agency { get; set; }
            public string geolocation_source { get; set; }
            public string delivery_preference { get; set; }
            public object node { get; set; }
            public string street_number { get; set; }
            public string comment { get; set; }
            public Neighborhood1 neighborhood { get; set; }
            public DateTime? geolocation_last_updated { get; set; }
            public string receiver_phone { get; set; }
        }

        public class Country1
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class City1
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Municipality1
        {
            public object id { get; set; }
            public object name { get; set; }
        }

        public class State1
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Neighborhood1
        {
            public object id { get; set; }
            public string name { get; set; }
        }

        public class Substatus_History
        {
            public DateTime date { get; set; }
            public string substatus { get; set; }
            public string status { get; set; }
        }

        public class Shipping_Items
        {
            public int quantity { get; set; }
            public Dimensions_Source dimensions_source { get; set; }
            public string description { get; set; }
            public string id { get; set; }
            public object bundle { get; set; }
            public string user_product_id { get; set; }
            public int sender_id { get; set; }
            public string dimensions { get; set; }
        }

        public class Dimensions_Source
        {
            public string origin { get; set; }
            public object id { get; set; }
        }
    }
}