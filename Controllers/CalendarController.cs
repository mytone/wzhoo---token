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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace wzhoo.Controllers
{
    public class CalendarController : Controller
    {

        /// <summary>
        /// 
        ///  update time: 2020-10-30
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 
        /// calendar table list    
        /// Timestyle:GMT,western european london...    
        /// update time: 2020-10-30
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OnCalendarList(string datetime)
        {
            string Datetime;
            if (datetime == "" || datetime == null)
            {
                //UTC GMT 0 dealing   local:GMT+8;
                Datetime = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd");
            }
            else
            {
                Datetime = HttpUtility.UrlDecode(datetime).ToString();
            }
            Console.WriteLine(Datetime);
            string strr = "select * from calendar where datetime='"+Datetime+"' order by datetime,time ";
            
            MySqlContext mys = new MySqlContext();
            List<NewTableCalendar> ltpi = new List<NewTableCalendar>();
            try
            {
                foreach (var item in mys.GetSqlDatas(strr))
                {
                    NewTableCalendar vv = new NewTableCalendar();
                    vv.id = item["id"].ToString();
                    vv.datetime = item["datetime"].ToString().Replace(" 0:00:00", "");
                    vv.time = item["time"];
                    vv.currency = item["currency"];
                    vv.eventz = item["event"];
                    vv.actual = item["actual"];
                    vv.forecast = item["forecast"];
                    vv.previous = item["previous"];
                    ltpi.Add(vv);
                }
            }
            catch (Exception)
            {

            }
            return Json(ltpi);
        }



        public IActionResult VarietyCalendar(string variety)
        {
            ViewBag.variety = HttpUtility.UrlDecode(variety);
            //Console.WriteLine(HttpUtility.UrlDecode(variety));
            return View();
        }

        /// <summary>
        /// table list calendar  on ajax
        /// </summary>
        /// <param name="variety"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OnTableListCalendar(string variety, int pageno)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            List<NewTableCalendar> ltPI = new List<NewTableCalendar>();
            string vstr = "";
            switch (pageno)
            {
                case 99999:
                    vstr = "select * from calendar where event='" + Variety + "'order by datetime desc ";
                    break;
                case 30:
                    vstr = "select * from calendar where event='" + Variety + "' and datetime>DATE_SUB(CURDATE(), INTERVAL 2 MONTH) order by datetime desc"; // 一年;
                    break;
                case 365:
                    vstr = "select * from calendar where event='" + Variety + "' and datetime>DATE_SUB(CURDATE(), INTERVAL 1 YEAR) order by datetime desc"; // 一年;
                    break;
                case 1080:
                    vstr = "select * from calendar where event='" + Variety + "' and datetime>DATE_SUB(CURDATE(), INTERVAL 3 YEAR) order by datetime desc"; // 一年;
                    break;
                case 1800:
                    vstr = "select * from calendar where event='" + Variety + "' and datetime>DATE_SUB(CURDATE(), INTERVAL 5 YEAR) order by datetime desc"; // 一年;
                    break;
                case 3650:
                    vstr = "select * from calendar where event='" + Variety + "' and datetime>DATE_SUB(CURDATE(), INTERVAL 10 YEAR) order by datetime desc"; // 一年;
                    break;
            }
            Console.WriteLine(vstr);
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas(vstr))
                {
                    NewTableCalendar vv = new NewTableCalendar();
                    vv.id = item["id"];
                    vv.datetime = item["datetime"].ToString().Replace("0:00:00", "");
                    vv.time = item["time"].ToString();
                    vv.currency = item["currency"];
                    vv.eventz = item["event"];
                    vv.forecast = item["forecast"].ToString();
                    //vv.previous = item["prevoious"].ToString();
                    vv.actual = item["actual"].ToString();
                    ltPI.Add(vv);
                }
            }
            catch (Exception)
            {
            }
            return Json(ltPI);
        }




    }
}