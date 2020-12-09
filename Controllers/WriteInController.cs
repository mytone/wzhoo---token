using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wzhoo.Models;
using MySql.Data.MySqlClient;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace wzhoo.Controllers
{
    public class WriteInController : Controller
    {    
        //[Authorize]
        public IActionResult Index()
        {
            return View();
        }

        //dy input content///////////
        [Authorize]
        [HttpGet]
        public string OnInput()
        {            
            string email=null;
            if (HttpContext.User.Claims.Count() > 0)
            {
                email=HttpContext.User.Claims.First(d => d.Type == ClaimTypes.Email)?.Value;
            }
            if (email != "zou_chao824@163.com")
            {
                return "Error";
            }
            string strr = "";
            strr = strr + "Content URL:&nbsp;&nbsp;";
            strr = strr + "<input id='sendcontent' type='text' class='form-control' /><br/>";
            strr = strr + "<input id='sendsubmit'  type='button' class='btn btn-success' value='Submit' />";
            return strr;
        }
    }
}