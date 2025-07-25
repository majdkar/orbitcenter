﻿using SchoolV01.Application.Configurations;
using SchoolV01.Application.Interfaces.Services.Identity;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using SchoolV01.Shared.Constants.User;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Enums;
using SchoolV01.Shared.Constants.Role;
using Microsoft.Extensions.Logging;

namespace SchoolV01.Infrastructure.Services.Identity
{
    public class IdentityService(UserManager<BlazorHeroUser> userManager, RoleManager<BlazorHeroRole> roleManager,
        IOptions<AppConfiguration> appConfig, IStringLocalizer<IdentityService> localizer, ILogger<IdentityService> logger,
        IUnitOfWork<int> unitOfWork) : ITokenService
    {

        private readonly UserManager<BlazorHeroUser> _userManager = userManager;
        private readonly RoleManager<BlazorHeroRole> _roleManager = roleManager;
        private readonly AppConfiguration _appConfig = appConfig.Value;
        private readonly IStringLocalizer<IdentityService> _localizer = localizer;
        private readonly ILogger<IdentityService> _logger = logger;
        private readonly IUnitOfWork<int> _unitOfWork = unitOfWork;

        public async Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.EmailOrUserName);

            if (user is null)
            {
                try
                {
                    user = await _userManager.FindByEmailAsync(model.EmailOrUserName);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception occurred: {Message}, Time of occurrence {time}", e.Message, DateTime.UtcNow);
                    return await Result<TokenResponse>.FailAsync(_localizer["Login By User Name."]);
                }
            }
            if (user is null)
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);

            if (!user.IsActive)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Active. Please contact the administrator."]);
            }
            if (!user.EmailConfirmed)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["E-Mail not confirmed."]);
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Credentials."]);

            var client = await GetClientIdAsync(user);
            if (client.Item1 == 0)
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            IEnumerable<Claim> claims = [new Claim("userId",user.Id), new Claim("vehicleId", "vehicle-123"),new Claim("clientId", client.Item1.ToString()), new Claim("clientId2", client.Item2.ToString())];
            var token = await GenerateJwtAsync(user, claims);


            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                UserImageURL = user.PictureUrl,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id,
                ClientId = client.Item1,
                vehicleId = "vehicle-123",
                Id = GetIdForUser(user.Id),
                ClientType = !string.IsNullOrEmpty(user.ClientType) ? user.ClientType : "Not Type Or Admin",
                Roles = string.Join(",", await _userManager.GetRolesAsync(user))
            };

            return await Result<TokenResponse>.SuccessAsync(response);
        }


        private async Task<(int?, int)> GetClientIdAsync(BlazorHeroUser user)
        {
            switch (user.ClientType)
            {
               
                
                
                default:
                    return (null, 0);
            }
        }

        //private async Task<(int?, int)> GetClientIdAsync(BlazorHeroUser user)
        //{
        //    switch (user.ClientType)
        //    {
        //        case RoleConstants.StudentRole:
        //            var student = await _unitOfWork.Repository<Student>().Entities
        //                .Where(c => c.UserId == user.Id)
        //                .Select(c => new
        //                {
        //                    c.Id,
        //                    c.Status,
        //                    StudentEnrollment = c.StudentEnrollments.FirstOrDefault(x => x.Season.IsActive)
        //                })
        //                .FirstOrDefaultAsync();
        //            var allow = (student?.Status == StudentStatus.Pending || student?.Status == StudentStatus.Accepted);
        //            return (allow ? student.StudentEnrollment?.Id ?? student?.Id ?? 0 : 0, student?.Id ?? 0);
        //        case RoleConstants.GuardianRole:
        //            var guardian = await _unitOfWork.Repository<Guardian>().Entities
        //                .Where(c => c.UserId == user.Id)
        //                .AsNoTracking()
        //                .FirstOrDefaultAsync();
        //            //var allow = (student?.Status == StudentStatus.Pending || student?.Status == StudentStatus.Accepted) && student?.State == StudentState.Register;
        //            return (guardian?.Id ?? 0, 0);

        //        case RoleConstants.EmployeeRole:
        //            var employee = await _unitOfWork.Repository<Employee>().Entities
        //                .Where(c => c.UserId == user.Id)
        //                .AsNoTracking()
        //                .FirstOrDefaultAsync();
        //            return (employee?.Id ?? 0, 0);

        //        default:
        //            return (null, 0);
        //    }
        //}

        public string GetTypeClient(string userId)
        {

            if (userId != null)
            {

                return "Not Type Or Admin";
            }
            else
            {
                return "Not Type Or Admin";
            }
        }

        public int GetClientId(string userId)
        {
            //var person = _unitOfWork.Repository<Person>().Entities
            //    .AsNoTracking()
            //    .Include(p => p.User)
            //    .FirstOrDefault(p => p.User.Id == userId);

            return  0;
        }

        public int GetIdForUser(string userId)
        {
            return 0;
            /*
            var person = _unitOfWork.Repository<Person>().Entities
                .AsNoTracking()
                .Include(p => p.User)
                .FirstOrDefault(p => p.User.Id == userId);

            int id = 0;

            switch (person)
            {
                case Employee employee:
                    id = employee.Id;
                    break;

                case Student student:
                    id = student.Id;
                    break;

                default:
                    id = 0;
                    break;
            }

            return id;
            */
        }

        public async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
        {
            if (model is null)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Client Token."]);
            }
            var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);
            if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Client Token."]);
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            user.RefreshToken = GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
            return await Result<TokenResponse>.SuccessAsync(response);
        }

        private async Task<string> GenerateJwtAsync(BlazorHeroUser user, IEnumerable<Claim> claims)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), (await GetClaimsAsync(user)).Concat(claims));
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(BlazorHeroUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddDays(2),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException(_localizer["Invalid token"]);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}