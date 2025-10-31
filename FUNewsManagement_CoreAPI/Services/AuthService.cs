using FUNewsManagement_CoreAPI.DTOs;
using FUNewsManagement_CoreAPI.Models;
using FUNewsManagement_CoreAPI.Repositories.Interfaces;
using FUNewsManagement_CoreAPI.Services.Interfaces;

namespace FUNewsManagement_CoreAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISystemAccountRepo _accountRepo;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshRepo;
        public AuthService(ISystemAccountRepo accountRepo,
            JwtService jwtService,
            IConfiguration config,
            IRefreshTokenRepository refreshRepo)
        {
            _accountRepo = accountRepo;
            _jwtService = jwtService;
            _config = config;
            _refreshRepo = refreshRepo;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO req)
        {
                if (req.Email.Equals(_config["AdminAccount:Email"])
                && req.Password.Equals(_config["AdminAccount:Password"]))
            {
                var admin = new SystemAccount
                {
                    AccountId = 0,
                    AccountEmail = _config["AdminAccount:Email"],
                    AccountRole = 999
                };
                //var refreshToken = _jwtService.GenerateRefreshToken();
                //await _refreshRepo.AddAsync(new RefreshToken
                //{
                //    UserId = 0,
                //    Token = refreshToken,
                //    ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_config["RefreshTokenExpiration"])),
                //    IsRevoked = false,
                //});
                await _refreshRepo.SaveChangesAsync();
                return new LoginResponseDTO
                {
                    AccessToken = _jwtService.GenerateAccessToken(admin),
                    RefreshToken = null,
                    Account = new AccountDTO
                    {
                        Email = req.Email,
                        Name = "Admin",
                        Role = 999
                    }
                };
            }
            var user = await _accountRepo.LoginUser(req);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            var rfToken = _jwtService.GenerateRefreshToken();
            await _refreshRepo.AddAsync(new RefreshToken
            {
                UserId = user.AccountId,
                Token = rfToken,
                ExpireAt = DateTime.UtcNow.AddDays(double.Parse(_config["RefreshTokenExpiration"])),
                IsRevoke = false,
            });
            await _refreshRepo.SaveChangesAsync();
            return new LoginResponseDTO
            {
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = rfToken,
                Account = new AccountDTO
                {
                    Email = user.AccountEmail,
                    Name = user.AccountName,
                    Role = user.AccountRole
                }
            };
        }
    }
}
