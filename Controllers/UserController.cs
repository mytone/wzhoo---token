using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wzhoo.Models;
using MySql.Data.MySqlClient;
using System.Web;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace wzhoo.Controllers
{
    public class UserController : Controller
    {

        [HttpGet]
        public IActionResult Add(string variety)
        {
            ViewBag.variety = HttpUtility.UrlDecode(variety).ToString();
            List<UserList> ltPI = new List<UserList>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select count(id) as ccc from user where variety='" + variety + "'"))
                {
                    ViewBag.count = item["ccc"].ToString();
                }
            }catch(Exception ex)
            {

            }
            return View();
        }

        public IActionResult AddDeal(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string name = HttpUtility.UrlDecode(user.name).ToString();
            string email = HttpUtility.UrlDecode(user.email).ToString();
            string content = HttpUtility.UrlDecode(user.content).ToString();
            string variety = user.variety.ToString();
            DateTime dt = DateTime.Now;
            try
            {
                MySqlContext my = new MySqlContext();
                my.Execute("insert into user(content,name,email,createtime,variety)values('" + content + "','" + name + "','" + email + "','" + dt.ToLocalTime().ToString()+ "','"+variety+"')");                
            }
            catch(Exception ex)            
            {

            }
            return (Redirect("/User/Add?variety=" + variety));
        }

        public JsonResult OnContentAdd(string variety)
        {
            string Variety = variety.ToString().ToUpper();
            List<User> ltPI = new List<User>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select * from user where variety='"+Variety+"' order by createtime desc"))
                {
                    User user = new User();
                    user.id = item["id"].ToString();
                    user.createtime = item["createtime"].ToString();
                    user.content = item["content"].ToString();
                    user.name = item["name"].ToString();
                    ltPI.Add(user);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }


        public IActionResult Login()
        {
            return View();
        }

        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // user login /////// return token///////////////////// 
        [AllowAnonymous]
        [HttpPost]
        public string OnUserLoginDeal(UserInfo userinfo)
        {
            string name = HttpUtility.UrlDecode(userinfo.name).ToString();
            string email = HttpUtility.UrlDecode(userinfo.email).ToString();
            string password = HttpUtility.UrlDecode(userinfo.password).ToString();

            List<UserInfo> ltpi = new List<UserInfo>();
            MySqlContext mys = new MySqlContext();
            string strr = "insert into userinfo(name,email,password,createtime)values('" + name + "','" + email + "','" + password + "','" + DateTime.Now + "')";
            mys.Execute(strr);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email,email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSetting:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "AESCR",
                audience: "AESCR",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        /// <summary>       
        /// index page loading  ///  token valid  ///   updatetime 20201207   
        /// _Layout actoken sending 
        /// </summary>
        /// <returns></returns>
        ///[Authorize]
        [HttpPost]
        public JsonResult VldToken()
        {
            string email;
            string createtime;
            string id;
            string accountz;
            string accountzinfo;
            List<string> lpt = new List<string>();
            try
            {
                if (HttpContext.User.Claims.Count()!= 0)
                {
                    email = HttpContext.User.Claims.First(d => d.Type == ClaimTypes.Email)?.Value;
                    createtime = HttpContext.User.Claims.First(d => d.Type == ClaimTypes.Role)?.Value;
                    id = HttpContext.User.Claims.First(d => d.Type == ClaimTypes.PrimarySid)?.Value;
                    accountz = "<img src='/img/my.png' style='width: 20px; height: 20px; '/>|&nbsp;"+email;
                    accountzinfo = "";
                    if (email == "zou_chao824@163.com")
                    {
                        accountzinfo = accountzinfo + "<a class='dropdown-item' href='/Title/Add' style='color:#333; text-decoration:none;' ><img src='/img/发布.png' style='width: 20px; height: 20px;'/>&nbsp;&nbsp;News feed </a>";         
                        accountzinfo = accountzinfo + "<div class='dropdown - divider'></div>";
                    }
                    else
                    {
                        accountzinfo = accountzinfo + "<a class='dropdown-item' href='#' style='color:#333; text-decoration:none;' ><img src='/img/发布.png' style='width: 20px; height: 20px;'/>&nbsp;&nbsp;News feed / Temporarily out of function</a>";
                        accountzinfo = accountzinfo + "<div class='dropdown - divider'></div>";
                    }                                    
                    accountzinfo = accountzinfo + "<a class='dropdown-item' href='#' style='color:#333; text-decoration:none;' onclick='outsubmit();'><img src='/img/退出.png' style='width: 22px; height: 22px;'/>&nbsp;&nbsp;Logout</a>";
                }
                else
                {
                    id = null;
                    email = null;
                    createtime = null;
                    accountz = "Account";
                    accountzinfo = "";
                    accountzinfo = accountzinfo + "<a class='dropdown-item' id = 'vv' href='#' style='color:#333; text-decoration:none;' data-toggle='modal' data-target='.bs-example-modal-sm'><img src='/img/登录.png' style='width: 22px; height: 22px;'/>&nbsp;&nbsp;Login</a>";
                    accountzinfo = accountzinfo + "<a class='dropdown-item' href='/Home/About' style='color:#333; text-decoration:none;'><img src='/img/logo2.png' style='width: 24px; height: 24px;'/>&nbsp;&nbsp;About</a>";
                }
                lpt.Add(accountz);          //0
                lpt.Add(accountzinfo);      //1
                lpt.Add(id);                //2
                lpt.Add(email);             //3
                lpt.Add(Convert.ToDateTime(createtime).ToString("yyyy-MM-dd"));  //4


            }catch(Exception ex)
            {
            }          
            return Json(lpt);
        }



    }
}