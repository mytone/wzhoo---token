using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using System.IO;
using wzhoo.Models;
using System.Web;
using Microsoft.AspNetCore.Cors;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;
using wzhoo.Apps;

namespace wzhoo.Controllers
{

    public class TitleController : Controller
    {

        /// <summary>
        /// 
        /// title content addd   ///  view page 
        /// 
        /// </summary>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        /// <summary>
        /// 
        /// title content adddeal  ///  webapi out   
        /// 
        /// </summary>
        [Authorize]
        [HttpPost]
        public string OnAddDeal(string title,string contenten,string contentcn,string pass)
        {
            //"LJHnLKaJ_7868hkjhk9760gjk_96970HJHLKHKLHsdfs9879870")
            string email;
            string createtime;
            string userid; 
            if (HttpContext.User.Claims.Count() != 0)
            {
                email = HttpContext.User.Claims.First(d => d.Type == ClaimTypes.Email)?.Value;
                createtime = HttpContext.User.Claims.First(d => d.Type == ClaimTypes.Role)?.Value;
                userid = HttpContext.User.Claims.First(d => d.Type == ClaimTypes.PrimarySid)?.Value;
            }
            else
            {
                userid = null;
                email = null;
                createtime = null;
            }

            if (email != "zou_chao824@163.com")
            {
                return ("error");
            }

            string stx = "";
            string datetimez = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string titlez = HttpUtility.UrlDecode(title);
            string contentenz = HttpUtility.UrlDecode(contenten).Trim();
            string contentcnz = HttpUtility.UrlDecode(contentcn).Trim();
            CommonTool ct = new CommonTool();
            stx = ct.GetVarietyLabel(contentenz);

            MySqlContext mys = new MySqlContext();
            try
            {                
                string strr = "insert into title(title,contenten,userid,datetime,contentcn,label)values('" + titlez + "','" + contentenz + "'," + userid + ",'" + datetimez + "','"+contentcnz+"','"+stx+"')";
                mys.Execute(strr);
                //strr = "select id,userid,datetime from title where userid=" + userid + " and datetime='" + datetimez + "'";
                //List<string> ltPI = new List<string>();
                //foreach(var item in mys.GetSqlDatas(strr))
                //{
                //    ltPI.Add(item["id"].ToString());
                //}
                //string idx = ltPI[0];          
  
                return ("ok");

            }
            catch(Exception ex)
            {
                return ("error");
            }

        }


        /// <summary>
        /// 
        /// list title of page index on deal 
        /// 
        /// </summary>
        /// <param name="titlepageno"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OnTitleList(int pageno,string lang)
        {
            int pagecount = 6;
            string strr = "";
            string fpage = ((pageno-1) * pagecount).ToString();
            string ppage = (pageno * pagecount).ToString();
            string langz= HttpUtility.UrlDecode(lang);
            List <TableTitle> ltPI = new List<TableTitle>();

            try
            {
                MySqlContext mys = new MySqlContext();
                strr = "select * from title order by datetime desc limit "+fpage+","+ppage+"";                
                foreach (var item in mys.GetSqlDatas(strr))
                {
                    TableTitle vv = new TableTitle();
                    vv.id = item["id"].ToString();
                    if (langz == "en")
                    {
                        vv.contenten = item["contenten"].ToString();
                    }
                    if(langz == "cn")
                    {
                        vv.contenten = item["contentcn"].ToString();
                    }
                    else
                    {
                        vv.contenten = item["contenten"].ToString();
                    }
                    vv.label = item["label"].ToString();
                    vv.datetime = Convert.ToDateTime( item["datetime"]).ToString("yyyy-MM-dd hh:mm");
                    vv.userid = item["userid"].ToString();
                    
                    ltPI.Add(vv);
                }                
            }
            catch(Exception)
            {
            }
            return Json(ltPI);
        }


        [HttpPost]
        public string reportupimage(List<IFormFile> files)
        {

            if (files.Count < 1)
            {
                return "error:empty.";
            }

            Console.WriteLine(files.Count.ToString());
            return "ok";
        }






    }
}