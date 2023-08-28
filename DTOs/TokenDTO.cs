namespace FinanceTrackerAPI.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; } = string.Empty;


        public TokenDTO(string token)
        {
            Token = token;
        }
    }
}
