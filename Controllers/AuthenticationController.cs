using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;

namespace wzhoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticateService _authService;
        public AuthenticationController(IAuthenticateService authService)
        {
            this._authService = authService;
        }

        /// <summary>
        /// userlogin token deal 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        [HttpPost, Route("requestToken")]
        public string requestToken([FromBody] LoginRequestDTO request)  /////login deal
        {
            if (!ModelState.IsValid)
            {
                return ("Invalid Request");
            }

            string token;
            if (_authService.IsAuthenticated(request, out token))
            {      
                return (token);
            }
            return ("error");
        }

        //[AllowAnonymous]
        [HttpPost, Route("registerToken")]
        public string registerToken([FromBody] LoginRequestDTO request)  /////register deal
        {
            if (!ModelState.IsValid)
            {
                return ("Invalid Request");
            }
            string strr;
            string token;
            switch(_authService.AreAuthenticated(request, out token))
            {
                case "userexist": strr= "userexist"; break;
                case "false": strr="false"; break;
                case "true": strr=token; break;
                default: strr = "false"; break;                     
            }
            return (strr);
       }


    }
}