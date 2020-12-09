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
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace wzhoo.Controllers
{
    public class DetailedController : Controller
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variety"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IndexDay(string variety)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            ViewBag.level = "day".ToUpper();
            ViewBag.variety = Variety.ToUpper();
            return View();
        }


        [HttpGet]
        public IActionResult IndexDayMax(string variety)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            ViewBag.level = "day".ToUpper();
            ViewBag.variety = Variety.ToUpper();
            return View();
        }


        /// <summary>
        /// detailed table data list   level minute /////
        /// </summary>
        /// <param name="variety"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IndexMinute(string variety,string datez)
        {
            string Datez= HttpUtility.UrlDecode(datez);
            string Variety = HttpUtility.UrlDecode(variety);
            ViewBag.level = "minute".ToUpper();
            ViewBag.variety = Variety.ToUpper();
            ViewBag.datetime = Datez;
            return View();
        }


        [HttpGet]
        public IActionResult IndexCrudeOilCost()
        {
            return View();
        }

        [HttpGet]
        public IActionResult IndexECBInterestRates()
        {
            return View();
        }


        /// <summary>
        /// general table   columns ->  data -> json
        /// </summary>
        /// <param name="variety"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OnIndexGeneral(string variety)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            MySqlContext mys = new MySqlContext();
            List<Dictionary<string, string>> vv = new List<Dictionary<string, string>>();
            try
            {
                vv = mys.GetSqlDatas("select * from " + Variety + " order by datetime desc");                
            }catch(Exception ex)
            {

            }
            return Json(vv);
        }


        

        /// <summary>
        /// financicel calendar index page view 
        /// </summary>
        /// <param name="variety"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IndexCalendar(string variety)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            ViewBag.variety = Variety;
            return View();
        }


 

        /// day anaylsis
        [HttpGet]
        public IActionResult IndexDayAnaylsis(string variety)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            ViewBag.variety = Variety.ToUpper();
            return View();
        }



        /// <summary>
        /// common information  ajax      variety  ecb 
        /// </summary>
        /// <param name="variety"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpPost] /////page  state
        public List<string> OnStatistics(string variety,string level)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            string Level = "";
            if (level != null && level!="")
            {
                Level = "_" + level;
            }
            else
            {
                level = "";
            }
            string strr = "";
            List<string> ltPI = new List<string>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select count(id) as ccc,max(DateTime) as maxdatetime, min(DateTime) as mindatetime from " + Variety + Level + " "))
                {
                    ltPI.Add(item["ccc"].ToString());
                    ltPI.Add(item["maxdatetime"].Replace("0:00:00","").ToString());
                    ltPI.Add(item["mindatetime"].Replace("0:00:00", "").ToString());
                }
                foreach (var itemz in my.GetSqlDatas("select * from indexvariety where name='"+Variety+"'")) //("show table status from tradedata where name='" + Variety + "'"))
                {
                    strr = strr + itemz["productname"];
                    strr = strr + " " + itemz["subgroup"];
                    strr = strr + " " + itemz["clearedas"];
                    strr = strr + " " + itemz["Exchange"];
                    ltPI.Add(strr);
                }
            }
            catch (Exception ex)
            {
            }
            return (ltPI);
        }


        /// <summary>
        /// common information  ajax      fcdata
        /// </summary>
        /// <param name="variety"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpPost] /////page  state
        public List<string> OnFCStatistics(string variety, string level)
        {
            string Variety = HttpUtility.UrlDecode(variety);
            string strr = "";
            List<string> ltPI = new List<string>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select count(id) as ccc,max(DateTime) as maxdatetime, min(DateTime) as mindatetime from fcdata where nameen='"+Variety+"'"))
                {
                    ltPI.Add(item["ccc"].ToString());
                    ltPI.Add(item["maxdatetime"].Replace("0:00:00", "").ToString());
                    ltPI.Add(item["mindatetime"].Replace("0:00:00", "").ToString());
                }
                foreach (var itemz in my.GetSqlDatas("select * from indexvariety where name='" + Variety + "'"))
                {
                    strr = strr + itemz["productname"];
                    strr = strr + " " + itemz["subgroup"];
                    strr = strr + " " + itemz["clearedas"];
                    strr = strr + " " + itemz["Exchange"];
                    ltPI.Add(strr);
                }
            }
            catch (Exception ex)
            {
            }
            return (ltPI);
        }


        [HttpPost]  //ajax  day  minute
        public JsonResult OnTableListMinute(string variety,string datez)
        {
            //string firstno = (pageno * 10).ToString();
            //string endno = (pageno * 10 + 500).ToString();
            string Variety = HttpUtility.UrlDecode(variety).ToString();
            string Datez="";
            string strr="";
            //Console.WriteLine(datez);
            if (datez == ""||datez==null)
            {
                Datez = DateTime.Now.ToString("yyyy-mm-dd ");
                strr = "select id,DateTime,Time,Open,Close,High,Low,TotalVolume from " + Variety+"  where DateTime=(select max(datetime) from "+Variety+") order by Time asc ";
            }
            else
            {
                Datez=HttpUtility.UrlDecode(datez).ToString(); 
                strr = "select id,DateTime,Time,Open,Close,High,Low,TotalVolume from " + Variety + " where DateTime='"+Datez+"' order by Time asc ";
            }
            List<VarietyTable> ltPI = new List<VarietyTable>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas(strr))
                {
                    VarietyTable vv = new VarietyTable();
                    vv.id = item["id"].ToString();
                    vv.datetime = item["DateTime"].Replace("0:00:00","").ToString();   //.Replace("0:00:00", "");
                    vv.time = item["Time"].ToString();
                    vv.open = item["Open"].ToString();
                    vv.close = item["Close"].ToString();
                    vv.high = item["High"].ToString();
                    vv.low = item["Low"].ToString();
                    vv.totalvolume = item["TotalVolume"].ToString();
                    ltPI.Add(vv);
                }
            }catch(Exception ex)
            {
            }                
            return Json(ltPI);
        }


        [HttpPost]  //ajax  tick
        public JsonResult OnTableListTick(string variety, int pageno)
        {
            string firstno = (pageno * 10).ToString();
            string endno = (pageno * 10 + 1000).ToString();
            string Variety = HttpUtility.UrlDecode(variety).ToString(); 
            List<VarietyTableTick> ltPI = new List<VarietyTableTick>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select * from " + Variety + " order by datetime desc limit " + firstno + "," + endno + ""))
                {
                    VarietyTableTick vv = new VarietyTableTick();
                    vv.id = item["id"].ToString();
                    vv.datetime = item["DateTime"].ToString();   //.Replace("0:00:00", "");
                    vv.price = item["Price"].ToString();
                    vv.totalvolume = item["TotalVolume"].ToString();
                    ltPI.Add(vv);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }


        /// <summary>
        /// 
        /// variety day   updatetime 20201114
        /// 
        /// </summary>
        /// <param name="variety"></param>
        /// <param name="pageno"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]  //ajax  day 
        public JsonResult OnTableListDay(string variety,int pageno, string fromdate, string todate)
        {            
            string Fromdate = HttpUtility.UrlDecode(fromdate).ToString();
            string Todate   = HttpUtility.UrlDecode(todate).ToString();
            string Variety  = HttpUtility.UrlDecode(variety).ToString();
            Console.WriteLine(Fromdate+Todate);
            List<VarietyTable> ltPI = new List<VarietyTable>();
            string vp = Variety.Split("_")[0];

            string vstr = "select * from " + Variety + " A left join history B on A.datetime=B.datetime and B.variety like '%"+vp+"%' ";
            vstr = vstr + " where A.datetime>'"+Fromdate+"' and A.datetime<'"+Todate+"'";

            /*
            switch (pageno){
                case 99999:
                    vstr = vstr+"";
                    break;
                case 7:
                    vstr = vstr + " where A.datetime>DATE_SUB(CURDATE(), INTERVAL 1 WEEK) "; // w;
                    break;
                case 30:
                    vstr = vstr + " where A.datetime>DATE_SUB(CURDATE(), INTERVAL 1 MONTH) "; // m;
                    break;
                case 365:
                    vstr = vstr + " where A.datetime>DATE_SUB(CURDATE(), INTERVAL 1 YEAR) "; // y
                    break;
                case 1080:
                    vstr = vstr + " where A.datetime>DATE_SUB(CURDATE(), INTERVAL 3 YEAR) "; // 3y
                    break;
                case 1800:
                    vstr = vstr + " where A.datetime>DATE_SUB(CURDATE(), INTERVAL 5 YEAR) "; //  5y
                    break;
            }*/

            vstr = vstr + " order by A.datetime desc";

            try
            {
                MySqlContext mys = new MySqlContext();
                foreach (var item in mys.GetSqlDatas(vstr))
                {
                    VarietyTable vv = new VarietyTable();
                    vv.id = item["id"].ToString();
                    vv.datetime = item["DateTime"].ToString().Replace("0:00:00","");   //.Replace("0:00:00", "");
                    vv.open = item["Open"].ToString();
                    vv.close = item["Close"].ToString();
                    vv.high = item["High"].ToString();
                    vv.low = item["Low"].ToString();
                    vv.totalvolume = item["TotalVolume"].ToString();
                    vv.eventz = item["eventz"];
                    ltPI.Add(vv);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }

        /// <summary>
        /// table list calendar  on ajax
        /// </summary>
        /// <param name="variety"></param>
        /// <returns></returns>
        [HttpPost]  
        public JsonResult OnTableListCalendar(string variety,int pageno)
        {
            string Variety = HttpUtility.UrlDecode(variety).ToString();
            List<TableCalendar> ltPI = new List<TableCalendar>();
            string vstr = "";
            switch (pageno)
            {
                case 99999:
                    vstr = "select * from fc A right join fcdata B on A.nameen=B.nameen where A.nameen='" + Variety + "'  order by DateTime desc ";
                    break;
                case 30:
                    vstr = "select * from fc A right join fcdata B on A.nameen=B.nameen where A.nameen='" + Variety + "' and B.datetime>DATE_SUB(CURDATE(), INTERVAL 2 MONTH) order by B.datetime desc"; // 一年;
                    break;
                case 365:
                    vstr = "select * from fc A right join fcdata B on A.nameen=B.nameen where A.nameen='" + Variety + "' and B.datetime>DATE_SUB(CURDATE(), INTERVAL 1 YEAR) order by B.datetime desc"; // 一年;
                    break;
                case 1080:
                    vstr = "select * from fc A right join fcdata B on A.nameen=B.nameen where A.nameen='" + Variety + "' and B.datetime>DATE_SUB(CURDATE(), INTERVAL 3 YEAR) order by B.datetime desc"; // 一年;
                    break;
                case 1800:
                    vstr = "select * from fc A right join fcdata B on A.nameen=B.nameen where A.nameen='" + Variety + "' and B.datetime>DATE_SUB(CURDATE(), INTERVAL 5 YEAR) order by B.datetime desc"; // 一年;
                    break;
            }

            //string strr =  order by datetime desc";
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas(vstr))
                {
                    TableCalendar vv = new TableCalendar();
                    vv.id = item["id"];
                    vv.datetime = item["datetime"].ToString().Replace("0:00:00", "");
                    vv.time = item["time"].ToString();
                    vv.nameen = item["nameen"].ToString();
                    vv.namecn = item["namecn"].ToString();
                    vv.contenten = item["contenten"].ToString();
                    vv.contentcn = item["contentcn"].ToString();
                    vv.y = item["y"].ToString();
                    vv.x = item["x"].ToString();
                    vv.label = item["label"].ToString();
                    ltPI.Add(vv);
                }
            }
            catch (Exception)
            {
            }
            return Json(ltPI);
        }


        //ajax general table      
        //according to colume field name and count , back to json of all data
        //[Authorize]
        [HttpPost]    
        public JsonResult OnTableListGeneral(string variety, int pageno)
        {

            string Variety = HttpUtility.UrlDecode(variety).ToString();
            List<Dictionary<string, string>> ltPI = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> kk = new List<Dictionary<string, string>>();
            string strr = "select COLUMN_NAME from information_schema.COLUMNS where table_name = '" + variety + "' order by ordinal_position ";
            int colcount = 0;
            try
            {
                MySqlContext mys = new MySqlContext();
                kk = mys.GetSqlDatas(strr);
                colcount = kk.Count;
                foreach (var item in mys.GetSqlDatas("select * from " + Variety + " order by datetime desc" ))
                {
                    Dictionary<string, string> vv = new Dictionary<string, string>();
                    for(int n = 0; n < colcount; n++)
                    {
                        vv[kk[n]["COLUMN_NAME"]] = item[kk[n]["COLUMN_NAME"]].ToString();
                    }
                    ltPI.Add(vv);
                }
                kk.Clear();                     
            }
            catch (Exception ex)
            {                
            }
            return Json(ltPI);
        }



        [HttpPost]  //ajax  day  anaylsis ....... direction upwardshadow downshadow entity 
        public JsonResult OnTableListDayAnaylsis(string variety)
        {
            string id;
            string datetime;
            float open;
            float close;
            float high;
            float low;
            int totalvolume;

            string Variety = HttpUtility.UrlDecode(variety).ToString();
            List<VarietyDayListAnaylsis> ltPI = new List<VarietyDayListAnaylsis>();

            float mi = this.minrunout(variety); 
            int openTemp=6960;
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select * from " + Variety + ""))
                {
                    VarietyDayListAnaylsis vv = new VarietyDayListAnaylsis();
                    id = item["id"].ToString();
                    datetime = item["DateTime"].ToString().Replace("0:00:00","");
                    open = Convert.ToSingle(item["Open"]);
                    close = Convert.ToSingle(item["Close"]);      //.Replace("0:00:00", "");
                    high = Convert.ToSingle(item["High"]);
                    low = Convert.ToSingle(item["Low"]);
                    totalvolume = Convert.ToInt32(item["TotalVolume"]);
                    if (close > open)
                    {
                        vv.direction = "1";
                        vv.upwardshadow = (int)Math.Round((high - close)/mi);
                        vv.entity = (int)Math.Round((close - open) / mi);
                        vv.downshadow = (int)Math.Round((open - low) / mi);
                    }
                    else
                    {
                        vv.direction = "-1";
                        vv.upwardshadow = (int)Math.Round((high - open) / mi);
                        vv.entity = (int)(0-Math.Round((open - close) / mi));
                        vv.downshadow = (int)(Math.Round((close - low) / mi));
                    }                    
                    vv.openV = (int)(Math.Round(open/mi)-openTemp);                    
                    vv.closeV = (int)(Math.Round(close / mi)-Math.Round(open/mi));
                    openTemp = (int)(Math.Round(close / mi));
                    vv.datetime = datetime;
                    vv.range = (int)Math.Round((high - low)/mi);
                    vv.volume = totalvolume;
                    ltPI.Add(vv);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }


        public float minrunout(string variety)
        {
            string Variety = variety.Replace("_","/");
            List<float> minrunout = new List<float>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select * from indexmenu where name='" + Variety + "'"))
                {
                    minrunout.Add(Convert.ToSingle(item["minrunout"]));
                }
            }
            catch (Exception)
            {
            }           
            return minrunout[0];
        }






        /// <summary>
        /// return recentlyDate of table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<string> OnTableRecentlyDate(string variety)
        {
            string Variety= HttpUtility.UrlDecode(variety).ToString();
            string strr = "select max(datetime) as md from " + Variety + "";
            List<string> ltPI = new List<string>();
            MySqlContext mys = new MySqlContext();
            foreach(var item in mys.GetSqlDatas(strr))
            {
                ltPI.Add(item["md"].ToString());
            }
            return (ltPI);
        }





    }    
}