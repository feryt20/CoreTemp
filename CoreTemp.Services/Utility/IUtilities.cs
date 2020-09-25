using CoreTemp.Data.DTOs.Token;
using CoreTemp.Data.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Services.Utility
{
    public interface IUtilities
    {
        string RemoveHtmlXss(string html);
        Task<string> GetDomainIpAsync(string domain);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        Task<string> GenerateJwtTokenAsync(User user, bool isRemember);

        Task<TokenResponseDto> GenerateNewTokenAsync(TokenRequestDto tokenRequestDto);
        Task<TokenResponseDto> CreateAccessTokenAsync(User user, string refreshToken);
        MyToken CreateRefreshToken(string clientId, string userId, bool isRemember);

        Task<TokenResponseDto> RefreshAccessTokenAsync(TokenRequestDto tokenRequestDto);

        string FindLocalPathFromUrl(string url);
        bool IsFile(IFormFile file);

    }
}
