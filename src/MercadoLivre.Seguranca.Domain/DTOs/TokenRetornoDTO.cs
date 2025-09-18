namespace MercadoLivre.DTOs
{
    public class TokenRetornoDTO
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
        public string user_id { get; set; }
        public string refresh_token { get; set; }
    }
}