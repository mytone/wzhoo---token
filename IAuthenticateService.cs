using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wzhoo.Models;

namespace wzhoo
{
    public interface IAuthenticateService
    {
        bool   IsAuthenticated(LoginRequestDTO request, out string token);
        string AreAuthenticated(LoginRequestDTO request, out string token);
    }

    public class LoginRequestDTO
    {
        //[Required]
        //[JsonProperty("id")]
        //public string Id { get; set; }

        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }


        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }


        /// <summary>
        /// token login 认证服务
        /// </summary>
        public class TokenAuthenticationService : IAuthenticateService
        {
            private readonly IUserService _userService;
            private readonly TokenManagement _tokenManagement;
            public TokenAuthenticationService(IUserService userService, IOptions<TokenManagement> tokenManagement)
            {
                _userService = userService;
                _tokenManagement = tokenManagement.Value;
            }

            // user login deal    make token
            public bool IsAuthenticated(LoginRequestDTO request, out string token)
            {
                Dictionary<string,string> infox = new Dictionary<string,string>();
                token = string.Empty;
                if(_userService.IsValid(request,out infox)=="false")  // userinfo is not valid
                {
                    return false;
                }
 
                var claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid,infox["id"].ToString()),                    
                    new Claim(ClaimTypes.Email,infox["email"].ToString()),
                    new Claim(ClaimTypes.Role,infox["createtime"].ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer, 
                    _tokenManagement.Audience, 
                    claims,
                    expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                return true;
            }

            //user register deal    make token
            public string AreAuthenticated(LoginRequestDTO request, out string token)
            {
                Dictionary<string, string> infox = new Dictionary<string, string>();
                token = string.Empty;
                switch ( _userService.AreValid(request, out infox))
                {
                    case "userexist":   // user are exist
                        token = "";
                        return "userexist";
                        break;
                    case "false":   // user are exist
                        token = "";
                        return "false";
                        break;
                    case "true":
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.PrimarySid,infox["id"].ToString()),
                            new Claim(ClaimTypes.Email,infox["email"].ToString()),
                            new Claim(ClaimTypes.Role,infox["createtime"].ToString())
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var jwtToken = new JwtSecurityToken(
                            _tokenManagement.Issuer,
                            _tokenManagement.Audience,
                            claims,
                            expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                            signingCredentials: credentials
                        );
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                        return "true";
                        break;
                    default:
                        return "false";
                        break;
                }

            }

        }

    }

}
