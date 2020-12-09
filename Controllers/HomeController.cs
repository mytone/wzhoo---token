using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using wzhoo.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Web;

namespace wzhoo.Controllers
{
    public class HomeController : Controller
    {
        //private IHttpContextAccessor _accessor;
        private readonly ILogger<HomeController> _logger;
    

        /// <summary>
        /// 
        ///  main index page of site  ///   view page
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string ipaddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();  //get ip client
            try
            {
                MySqlContext my = new MySqlContext();
                DateTime dt = DateTime.Now;
                my.Execute("insert into ipcomment (ipaddress,createtime)values('"+ipaddress+"','"+ dt.ToLocalTime().ToString() + "')");
            }
            catch(Exception ex)
            {
            }
            List<IndexMenu> ltPI = new List<IndexMenu>();
            try
            {
                MySqlContext my = new MySqlContext();
                string strr = "select * from indexmenu A right join indexvariety B on A.name=B.name order by A.createtime desc";
                foreach (var item in my.GetSqlDatas(strr))
                {
                    IndexMenu vv = new IndexMenu();
                    vv.id = item["id"].ToString();
                    vv.name = item["globex"].ToString();  
                    vv.comment = item["productname"]+" "+item["productgroup"];
                    vv.createtime = "/img/" + item["globex"].ToString().ToLower() + ".png"; ;
                    vv.link = item["link"];
                    vv.level = item["level"];
                    vv.dt = item["dt"].ToString();
                    ltPI.Add(vv);                    
                }
            }catch(Exception)
            {
            }
            return View(ltPI);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contactus()
        {
            return View();
        }

        public IActionResult Cookie()
        {
            return View();
        }


        public IActionResult Map()
        {
            List<IndexMenu> ltPI = new List<IndexMenu>();
            try
            {
                MySqlContext my = new MySqlContext();
                string strr = "select * from indexmenu A right join indexvariety B on A.name=B.name order by A.createtime desc";
                foreach (var item in my.GetSqlDatas(strr))
                {
                    IndexMenu vv = new IndexMenu();
                    vv.id = item["id"].ToString();
                    vv.name = item["globex"].ToString();
                    vv.comment = item["productname"] + " " + item["productgroup"];
                    vv.createtime = item["createtime"];
                    vv.link = item["link"];
                    vv.level = item["level"];
                    vv.dt = item["dt"].ToString();
                    ltPI.Add(vv);
                }
            }
            catch (Exception)
            {
            }
            return View(ltPI);
        }

        /// <summary>
        /// search index 
        /// </summary>
        /// <param name="searchtext"></param>
        /// <returns></returns>
        public IActionResult Search(string searchtext)
        {                         
            ViewBag.searchtext = HttpUtility.UrlDecode(searchtext);
            return View();
        }

        /// <summary>
        /// 
        /// list title of page index on deal 
        /// 
        /// </summary>
        /// <param name="titlepageno"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OnTitleSearchList(int pageno, string lang, string searchtext)
        {
            int pagecount = 18;
            string strr = "";
            string fpage = ((pageno - 1) * pagecount).ToString();
            string ppage = (pageno * pagecount).ToString();
            string langz = HttpUtility.UrlDecode(lang);
            string Searchtext = HttpUtility.UrlDecode(searchtext);
            List<TableTitle> ltPI = new List<TableTitle>();
            try
            {
                MySqlContext mys = new MySqlContext();
                strr = "select* from title where contenten like '%" + Searchtext + "%' order by datetime desc limit " + fpage + "," + ppage + "";
                foreach (var item in mys.GetSqlDatas(strr))
                {
                    TableTitle vv = new TableTitle();
                    vv.id = item["id"].ToString();
                    if (langz == "en")
                    {
                        vv.contenten = item["contenten"].ToString();
                    }
                    if (langz == "cn")
                    {
                        vv.contenten = item["contentcn"].ToString();
                    }
                    else
                    {
                        vv.contenten = item["contenten"].ToString();
                    }
                    vv.label = item["label"].ToString();
                    vv.datetime = Convert.ToDateTime(item["datetime"]).ToString("yyyy-MM-dd hh:mm");
                    vv.userid = item["userid"].ToString();

                    ltPI.Add(vv);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
