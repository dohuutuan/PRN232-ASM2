using FUNewsManagement_CoreAPI.DTOs;
using FUNewsManagement_CoreAPI.Models;

namespace FUNewsManagement_CoreAPI.Repositories.Interfaces
{
    public interface ISystemAccountRepo : IRepository<SystemAccount>
    {
        Task<SystemAccount?> LoginUser(LoginRequestDTO request);
    }
}
