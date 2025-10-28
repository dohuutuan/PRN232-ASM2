using FUNewsManagement_CoreAPI.DTOs;
using FUNewsManagement_CoreAPI.Models;
using FUNewsManagement_CoreAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_CoreAPI.Repositories
{
    public class SystemAccountRepo : Repository<SystemAccount>, ISystemAccountRepo
    {
        public SystemAccountRepo(FunewsManagementContext context) : base(context)
        {
        }
        public async Task<SystemAccount?> LoginUser(LoginRequestDTO req)
        {
            var user = await _context.SystemAccounts
                .FirstOrDefaultAsync(u => u.AccountEmail.Equals(req.Email) && u.AccountPassword.Equals(req.Password));
            return user;
        }
    }
}
