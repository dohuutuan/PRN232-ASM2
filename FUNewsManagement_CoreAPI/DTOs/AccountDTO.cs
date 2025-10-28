namespace FUNewsManagement_CoreAPI.DTOs
{
    public class AccountDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public int? Role { get; set; }
    }
}
