using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CustomerPortalWebApi.Services
{
    public class Checktokenservice : IChecktokenservice
    {
        private readonly JwtSettings _jwtSettings;

        public Checktokenservice(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public bool CheckToken(string Token, string usercode)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            if (username[0].Value.ToString() == usercode)
            {
                return true;
            }
            else if (username[1].Value.ToString() == "Super Admin" || username[1].Value.ToString() == "SystemAdmin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTokenForCustomer(string Token, string usercode)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            if (username[0].Value.ToString() == usercode && username[1].Value.ToString() == "Customer")
            {
                return true;
            }
            else if (username[1].Value.ToString() != "Customer")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTokenForCustomerByID(string Token, long id)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            if (username[2].Value.ToString() == id.ToString() && username[1].Value.ToString() == "Customer")
            {
                return true;
            }
            else if (username[1].Value.ToString() != "Customer")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTokenByID(string Token, long id)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            if (username[2].Value.ToString() == id.ToString())
            {
                return true;
            }
            else if (username[1].Value.ToString() == "SuperAdmin" || username[1].Value.ToString() == "SystemAdmin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Start Added By Sunil 11 nov 2021
        public string GetUserId(string Token)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            return username[2].Value;
        }
        public string GetUserRole(string Token)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            return username[1].Value;
        }

        public string GetUserCode(string Token)
        {
            var principal = GetPrincipalFromExpiredToken(Token);
            var username = principal.Claims.ToList();
            return username[0].Value;
        }

        //  End Added By Sunil 11 nov 2021
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}