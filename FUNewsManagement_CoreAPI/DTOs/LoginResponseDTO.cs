namespace FUNewsManagement_CoreAPI.DTOs
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public AccountDTO Account { get; set; }
    }
}
