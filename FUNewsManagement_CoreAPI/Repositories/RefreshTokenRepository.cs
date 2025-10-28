using FUNewsManagement_CoreAPI.Models;
using FUNewsManagement_CoreAPI.Repositories.Interfaces;

namespace FUNewsManagement_CoreAPI.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(FunewsManagementContext context) : base(context)
        {
        }
        
    }
}
