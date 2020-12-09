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


namespace wzhoo.Controllers
{
    public class AnaylsisController : Controller
    {

        /// <summary>
        /// two variety data are comparative analysis
        /// </summary>
        /// <param name="Variety"></param>
        /// <param name="Variety2"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpGet]  // ssec vs  a50  anaylsis
        public IActionResult IndexTwoVarietyAnaylsis(string Variety, string Variety2,string Level)
        {
            string variety   = HttpUtility.UrlDecode(Variety);
            string variety2  = HttpUtility.UrlDecode(Variety2);
            string level     = HttpUtility.UrlDecode(Level);
            ViewBag.variety  = variety.ToUpper();
            ViewBag.variety2 = variety2.ToUpper();
            ViewBag.level    = level.ToUpper();
            return View();
        }
        [HttpPost]  //ajax  ssec vs a50 anaylsis 
        public JsonResult OnTableTwoVarietyAnaylsisList(string variety, string variety2,string level,int pageno)
        {
            string Variety = HttpUtility.UrlDecode(variety).ToString();
            string Variety2 = HttpUtility.UrlDecode(variety2).ToString();
            string strr = "";
            string vstr = "";

            switch (pageno)
            {
                case 99999:
                    vstr = "order by A.DateTime desc ";
                    break;
                case 7:
                    vstr = "where A.datetime>DATE_SUB(CURDATE(), INTERVAL 1 WEEK) order by A.datetime desc"; // w;
                    break;
                case 30:
                    vstr = "where A.datetime>DATE_SUB(CURDATE(), INTERVAL 2 MONTH) order by A.datetime desc"; // m;
                    break;
                case 365:
                    vstr = "where A.datetime>DATE_SUB(CURDATE(), INTERVAL 1 YEAR) order by A.datetime desc"; // y
                    break;
                case 1080:
                    vstr = "where A.datetime>DATE_SUB(CURDATE(), INTERVAL 3 YEAR) order by A.datetime desc"; // 3y
                    break;
                case 1800:
                    vstr = "where A.datetime>DATE_SUB(CURDATE(), INTERVAL 5 YEAR) order by A.datetime desc"; //  5y
                    break;
            }
            List<TableTwoVarietyAnaylsis> ltPI = new List<TableTwoVarietyAnaylsis>();
            try
            {
                MySqlContext my = new MySqlContext();
                strr = strr + " SELECT A.id as id,A.DateTime as datetime ,A.Open as open,A.High as high,A.Low as low,A.Close as close,A.TotalVolume as totalvolume,B.DateTime as datetime2, B.Open AS open2,B.High AS high2,B.Low AS low2,B.Close AS close2,B.TotalVolume as totalvolume2 ";
                strr = strr + " FROM " + Variety+"_"+level + " A inner JOIN " + Variety2+"_"+level + " B ON A.DateTime = B.DateTime "+vstr;
                foreach (var item in my.GetSqlDatas(strr))
                {
                    TableTwoVarietyAnaylsis vv = new TableTwoVarietyAnaylsis();
                    vv.id = Convert.ToInt32(item["id"]).ToString();
                    vv.datetime = item["datetime"].ToString().Replace("0:00:00", "");
                    vv.open = item["open"].ToString();
                    vv.close = item["close"].ToString();
                    vv.high = item["high"].ToString();
                    vv.low = item["low"].ToString();
                    vv.totalvolume = item["totalvolume"];
                    vv.datetime2 = item["datetime2"].ToString().Replace("0:00:00", "");
                    vv.open2 = item["open2"].ToString();
                    vv.high2 = item["high2"].ToString();
                    vv.low2 = item["low2"].ToString();
                    vv.close2 = item["close2"].ToString();
                    vv.totalvolume2 = item["totalvolume2"];
                    ltPI.Add(vv);
                }
            }
            catch (Exception)
            {
            }
            return Json(ltPI);
        }



        [HttpGet]
        public IActionResult IndexTableFinAnaylsis(string Variety, string FinVariety,string Level,string Currency)
        {
            string variety = HttpUtility.UrlDecode(Variety);
            string finvariety = HttpUtility.UrlDecode(FinVariety);
            string level = HttpUtility.UrlDecode(Level);
            string currency = HttpUtility.UrlDecode(Currency);
            ViewBag.variety = variety.ToUpper();
            ViewBag.finvariety = finvariety.ToUpper();
            ViewBag.level = level.ToUpper();
            ViewBag.currency = currency.ToUpper();
            return View();
        }


        [HttpGet]
        public IActionResult CLvsEIA()
        {
            string variety = "CL";
            string finvariety = "EIA Crude Oil Stocks Change";
            string level = "day";
            string currency = "USD";
            ViewBag.variety = variety.ToUpper();
            ViewBag.finvariety = finvariety;
            ViewBag.level = level.ToUpper();
            ViewBag.currency = currency.ToUpper();
            return View();
        }

        [HttpGet]
        public IActionResult GCvsNFP()
        {
            string variety    = "GC";
            string finvariety = "Nonfarm Payrolls";
            string level      = "day";
            string currency   = "USD";
            ViewBag.variety   = variety.ToUpper();
            ViewBag.finvariety = finvariety;
            ViewBag.level     = level.ToUpper();
            ViewBag.currency  = currency.ToUpper();
            return View();
        }

        [HttpGet]
        public IActionResult DXvsNFP()
        {
            string variety = "DX";
            string finvariety = "Nonfarm Payrolls";
            string level = "day";
            string currency = "USD";
            ViewBag.variety = variety.ToUpper();
            ViewBag.finvariety = finvariety;
            ViewBag.level = level.ToUpper();
            ViewBag.currency = currency.ToUpper();
            return View();
        }

        [HttpGet]
        public IActionResult EURGDPqvs6E()
        {
            string variety = "6E";
            string finvariety = "GDP q/q";
            string level = "day";
            string currency = "EUR";
            ViewBag.variety = variety.ToUpper();
            ViewBag.finvariety = finvariety;
            ViewBag.level = level.ToUpper();
            ViewBag.currency = currency.ToUpper();
            return View();
        }

        /// <summary>
        /// 
        /// variety vs Calendar Fin       updatetime 20201115
        /// 
        /// </summary>
        /// <param name="variety"></param>
        /// <param name="finvariety"></param>
        /// <param name="level"></param>
        /// <param name="pageno"></param>
        /// <returns></returns>
        [HttpPost]  //ajax  day 
        public JsonResult OnTableFinAnaylsisList(string variety,string finvariety, string level, int pageno,string currency)
        {
            string Variety    = HttpUtility.UrlDecode(variety).ToString()+"_day";
            string FinVariety = HttpUtility.UrlDecode(finvariety).ToString();
            string Currency   = HttpUtility.UrlDecode(currency).ToString();
            string vstr = "";
            switch (pageno)
            {
                case 99999:
                    vstr = "order by a.datetime desc ,b.datetime desc ";
                    break;
                default:
                    vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL "+pageno+" DAY) order by a.datetime desc ,b.datetime desc";  // 
                    break;

                    /* case 7:
                         vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL 1 WEEK) order by a.datetime desc ,b.datetime desc";  // w;
                         break;
                     case 30:
                         vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL 2 MONTH) order by a.datetime desc,b.datetime desc"; // m;
                         break;
                     case 365:
                         vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL 1 YEAR) order by a.datetime desc,b.datetime desc";  // y
                         break;
                     case 1080:
                         vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL 3 YEAR) order by a.datetime desc,b.datetime desc";  // 3y
                         break;
                     case 1800:
                         vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL 5 YEAR) order by a.datetime desc,b.datetime desc";  //  5y
                         break;
                     case 3650:
                         vstr = "where a.datetime>DATE_SUB(CURDATE(), INTERVAL 10 YEAR) order by a.datetime desc,b.datetime desc";  //  5y
                         break;*/
            }

            List<TableFinAnaylsis> ltPI = new List<TableFinAnaylsis>();

            try
            {
                var stx = "select * from "+Variety+" a left join Calendar b on a.datetime=b.datetime_gmt_5 and b.currency='"+Currency+"' and b.event='" + FinVariety + "' "+vstr;
                MySqlContext my = new MySqlContext();
                foreach(var item in my.GetSqlDatas(stx))
                {
                    TableFinAnaylsis kk = new TableFinAnaylsis();
                    kk.datetime = item["datetime"].Replace("0:00:00","").Trim();// DateTime.Parse(ipem["datetime"]).AddHours(-5).ToString().Split(" ")[0].Trim(); // GMT0 -> GMT-5
                    kk.currency = item["currency"];
                    kk.eventz   = item["event"];
                    kk.forecast = item["forecast"];
                    kk.actual   = item["actual"];
                    kk.id       = item["id"].ToString();
                    kk.open     = item["Open"].ToString();
                    kk.close    = item["Close"].ToString();
                    kk.high     = item["High"].ToString();
                    kk.low      = item["Low"].ToString();
                    ltPI.Add(kk);
                }
            }
            catch (Exception)
            {
            }
            return Json(ltPI);
        }


        [HttpPost] // statistic  data   maxdata  mindata 
        public JsonResult OnStatisticsDay(string variety, int pageno)
        {
            string vstr = "";
            switch (pageno)
            {
                case 99999:
                    vstr = "";
                    break;
                case 7:
                    vstr = " where datetime>DATE_SUB(CURDATE(), INTERVAL 1 WEEK) "; 
                    break;
                case 30:
                    vstr = " where datetime>DATE_SUB(CURDATE(), INTERVAL 2 MONTH)"; 
                    break;
                case 365:
                    vstr = " where datetime>DATE_SUB(CURDATE(), INTERVAL 1 YEAR) "; 
                    break;
                case 1080:
                    vstr = " where datetime>DATE_SUB(CURDATE(), INTERVAL 3 YEAR) "; 
                    break;
                case 1800:
                    vstr = " where datetime>DATE_SUB(CURDATE(), INTERVAL 5 YEAR) "; 
                    break;
            }
            string Variety = HttpUtility.UrlDecode(variety).ToString();
            string strr = "(select High as sda,DateTime from "+Variety+vstr+ " order by CONVERT(High, DECIMAL(10,5)) desc limit 0,1) union (select Low as sda, DateTime from " + Variety+vstr+ " order by CONVERT(Low, DECIMAL(10,5))  limit 0, 1)";
            string strx = "select (convert(High,DECIMAL(10,5))-convert(Low,DECIMAL(10,5))) as maxd, DateTime from "+Variety+vstr+" order by CONVERT(High, DECIMAL(10,5))-convert(Low,DECIMAL(10,5)) desc limit 0, 1";
            List<Dictionary<string,string>> ltPI = new List<Dictionary<string, string>>();

            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas(strr))
                {
                    Dictionary<string,string> pp = new Dictionary<string,string>();
                    pp["sda"] = item["sda"].ToString().Replace("0:00:00", "");
                    pp["datetime"] = item["DateTime"].ToString().Replace("0:00:00", "");
                    ltPI.Add(pp);
                }
                foreach (var item in my.GetSqlDatas(strx))
                {
                    Dictionary<string, string> pp = new Dictionary<string, string>();
                    pp["sda"] = item["maxd"].ToString();
                    pp["datetime"] = item["DateTime"].ToString().Replace("0:00:00", "");
                    ltPI.Add(pp);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }


        /// <summary>
        /// level minute 
        /// </summary>
        /// <param name="variety"></param>
        /// <param name="pageno"></param>
        /// <returns></returns>
        [HttpPost] // statistic  data   maxdata  mindata 
        public JsonResult OnStatisticsMinute(string variety, string datez)
        {
            string vstr = "";
            string Variety = HttpUtility.UrlDecode(variety).ToString();
            string Datez = datez;
            string strr = "select max(convert(High,DECIMAL(10,5))) as hprice, min(convert(Low,DECIMAL(10,5))) as lprice  from " + Variety+" where datetime='"+datez+"'";
            List<Dictionary<string, string>> ltPI = new List<Dictionary<string, string>>();

            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas(strr))
                {
                    Dictionary<string, string> pp = new Dictionary<string, string>();
                    pp["hprice"]  = item["hprice"].ToString();
                    pp["lprice"]  = item["lprice"].ToString();
                    pp["mcprice"] = ( Convert.ToDouble( pp["hprice"] ) - Convert.ToDouble( pp["lprice"]) ).ToString("0.00000");
                    ltPI.Add(pp);
                }
                /*foreach (var item in my.GetSqlDatas(strx))
                {
                    Dictionary<string, string> pp = new Dictionary<string, string>();
                    pp["sda"] = item["maxd"].ToString();
                    pp["datetime"] = item["DateTime"].ToString().Replace("0:00:00", "");
                    ltPI.Add(pp);
                }*/
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }







        public IActionResult IndexFundAnaylsis()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult OnFundAnaylsisList()
        {
            
            //string name;
            //string code;
            //string createtime;
            //double scale;
            /*float oneyearrr;
            float oneyearv;
            float oneyearsr;
            float oneyearmd;
            float threeyearrr;
            float threeyearv;
            float threeyearsr;
            float threeyearmd;
            float fiveyearrr;
            float fiveyearv;
            float fiveyearsr;
            float fiveyearmd;
            float esyearrr;
            float esyearv;
            float esyearsr;
            float esyearmd;*/

            List<TableFund> ltPI = new List<TableFund>();
            MySqlContext my = new MySqlContext();
            string strr = "SELECT A.name , A.code,A.countcode , A.createtime , A.scale , B.countcode AS countcode_j,A.esyearrr ";
            strr = strr + "FROM fund_anaylsis_g_ppp B  RIGHT JOIN  fund_anaylsis_j_ppp A  ";
            strr = strr + "ON A.code = B.code ";
            strr = strr + "WHERE DATE(A.createtime)<'2016' ";
            DateTime cn = DateTime.Now;
            int ys;
            foreach (var item in my.GetSqlDatas(strr))
            {
                TableFund vv = new TableFund();
                vv.name = item["name"].ToString();
                vv.code= item["code"].ToString();
                vv.countcode = item["countcode"].ToString();
                ys = cn.Year - Convert.ToDateTime(item["createtime"]).Year;
                vv.createtime = ys.ToString();// item["createtime"].ToString().Replace("0:00:00","");
                vv.scale = Convert.ToDouble(item["scale"]);
                vv.countcode_j = item["countcode_j"].ToString();
                vv.esyearrr = Math.Ceiling(( Convert.ToSingle(item["esyearrr"])/ys *100)).ToString()+"%";
                ltPI.Add(vv);
                
            }
            return Json(ltPI);

        }




    }
}