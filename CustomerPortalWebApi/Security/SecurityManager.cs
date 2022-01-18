using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerPortalWebApi.Security
{
    public class SecurityManager
    {
        private readonly IUserMasterService _userMasterService;
        private readonly JwtSettings _jwtSettings;

        public SecurityManager(IUserMasterService userMasterService, JwtSettings jwtSettings)
        {
            _userMasterService = userMasterService;
            _jwtSettings = jwtSettings;
        }

        public UserAuthenticationObject ValidateUser(string UserCode, string Password)
        {
            UserAuthenticationObject obj = new UserAuthenticationObject();

            var UserDetails = _userMasterService.Login(UserCode, Password);
            obj = BuildUserAuthObject(UserDetails);

            return obj;
        }

        private UserAuthenticationObject BuildUserAuthObject(UserMaster userMaster)
        {
            UserAuthenticationObject obj = new UserAuthenticationObject();
            if (userMaster != null && userMaster.UserCodetxt != null && userMaster.UserCodetxt != "")
            {
                obj.IDbint = userMaster.Idbint;
                obj.UserCodetxt = userMaster.UserCodetxt;
                obj.UserTypetxt = userMaster.UserTypetxt;
                obj.Divisionvtxt = userMaster.Divisionvtxt;
                obj.UserNametxt = userMaster.UserNametxt;
                obj.IsAuthenticated = true;
                obj.BearerToken = BuildJwtToken(obj);
                obj.Flag = userMaster.Flag;
            }
            else
            {
                obj.IDbint = userMaster.Idbint;
                obj.UserCodetxt = userMaster.UserCodetxt;
                obj.UserTypetxt = userMaster.UserTypetxt;
                obj.Divisionvtxt = userMaster.Divisionvtxt;
                obj.UserNametxt = userMaster.UserNametxt;
                obj.IsAuthenticated = true;
                obj.BearerToken = "";//BuildJwtToken(obj);
                obj.Flag = userMaster.Flag;
            }

            return obj;
        }

        private String BuildJwtToken(UserAuthenticationObject authObj)
        {
            List<Claim> jwtClaim = new List<Claim>();
            jwtClaim.Add(new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(authObj.UserCodetxt)));
            jwtClaim.Add(new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(authObj.UserTypetxt)));
            jwtClaim.Add(new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(authObj.IDbint)));
            jwtClaim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            //var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: jwtClaim, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddMinutes(_jwtSettings.MinToExpiration), signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            //return new JwtSecurityTokenHandler().WriteToken(token);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var jwt = new JwtSecurityToken(issuer: "http://localhost:44335",
                audience: "CustomerPortalUsers",
                claims: jwtClaim, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //...
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}