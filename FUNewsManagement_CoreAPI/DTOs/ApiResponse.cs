namespace FUNewsManagement_CoreAPI.DTOs
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
