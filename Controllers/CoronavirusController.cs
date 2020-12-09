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
    public class CoronavirusController : Controller
    {

        /// <summary>
        /// 
        ///  update time: 2020-10-20
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 
        /// coronavirous table list    
        /// Timestyle:GMT,western european london...    
        /// update time: 2020-10-20
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OnCoronavirusList()
        {     
            
            string strr = "select * from  coronavirus order by datetime desc, area desc ";            
            MySqlContext mys = new MySqlContext();
            List<TableCoronavirous> ltpi = new List<TableCoronavirous>();
            try
            {
                foreach (var item in mys.GetSqlDatas(strr))
                {
                    TableCoronavirous vv = new TableCoronavirous();
                    vv.id = item["id"].ToString();
                    vv.datetime = item["datetime"].ToString().Replace(" 0:00:00", "");
                    vv.area = item["area"];
                    vv.totalcases = item["totalcases"];
                    //vv.newcases = item["newcases"];
                    vv.totaldeaths = item["totaldeaths"];
                    //vv.newdeaths = item["newdeaths"];
                    //vv.totalrecovered = item["totalrecovered"];
                    //vv.activecases = item["activecases"];
                    //vv.seriouscritical = item["seriouscritical"];
                    Console.WriteLine(item["totaldeaths"]);
                    ltpi.Add(vv);
                }
            }
            catch (Exception)
            {

            }
            Console.WriteLine(ltpi);
            return Json(ltpi);
        }




    }
}