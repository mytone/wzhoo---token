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
    public class MyController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.variety = "MoneyFlow";
            return View();
        }

        [HttpPost] /////page  state
        public JsonResult OnMoneyFlowAllView()
        {
            List<TableMoneyFlowAll> ltPI = new List<TableMoneyFlowAll>();
            try
            {
                MySqlContext my = new MySqlContext();
                foreach (var item in my.GetSqlDatas("select * from moneyflowallview"))
                {
                    TableMoneyFlowAll vv = new TableMoneyFlowAll();
                    vv.id = item["id"].ToString();                   
                    vv.status = item["status"].ToString();   //.Replace("0:00:00", "");
                    vv.platform = item["platform"].ToString();
                    vv.name = item["name"].ToString();
                    vv.type = item["type"];
                    vv.prerateyear = (Convert.ToSingle(item["prerateyear"])*100).ToString()+"%";
                    vv.planday = item["planday"].ToString();
                    vv.prerate = (Convert.ToSingle(item["prerate"])*100).ToString() + "%";
                    vv.cost = item["cost"].ToString();
                    vv.returnmoney = item["returnmoney"].ToString();
                    vv.buydatetime = item["buydatetime"].ToString().Replace("0:00:00", ""); 
                    vv.planreturndatetime = item["planreturndatetime"].ToString().Replace("0:00:00","");
                    vv.description = item["description"].ToString();
                    vv.returndatetime = item["returndatetime"].ToString();
                    vv.acreturn = item["acreturn"].ToString();
                    vv.description_f = item["description_f"].ToString();
                    ltPI.Add(vv);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(ltPI);
        }

    }
}