using FUNewsManagement_CoreAPI.DTOs;

namespace FUNewsManagement_CoreAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO req);
    }
}
