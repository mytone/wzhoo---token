using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wzhoo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json.Serialization;

namespace wzhoo.Controllers
{
    public class ApiController : Controller
    {
        /// <summary>
        /// 
        /// api variety data write day level                2020-10-8 update
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="datetime"></param>
        /// <param name="o"></param>
        /// <param name="h"></param>
        /// <param name="l"></param>
        /// <param name="c"></param>
        /// <param name="v"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        [HttpGet]
        public string OnVarietyDataWriteDay(string tablename,string datetime,string o, string h,string l,string c,string v,string sign)
        {
            if (sign != "mytone20050212")
            {
                return ("login error");
            }

            string Datetime = HttpUtility.UrlDecode(datetime);
            string Open = HttpUtility.UrlDecode(o);
            string High = HttpUtility.UrlDecode(h);
            string Low = HttpUtility.UrlDecode(l);
            string Close = HttpUtility.UrlDecode(c);
            string Totalvolume = HttpUtility.UrlDecode(v);
            string Tablename = HttpUtility.UrlDecode(tablename);

            try
            {
                MySqlContext mys = new MySqlContext();
                string strr = "select * from " + Tablename + " where Open='" + Open + "' and High='" + High + "' and Low='" + Low + "' and Close='" + Close + "'";
                List<Dictionary<string, string>> ltPI = new List<Dictionary<string, string>>();
                ltPI = mys.GetSqlDatas(strr);
                if (ltPI.Count > 0)
                {
                    return ("This data exciting Have");
                }
                strr = "insert into " + Tablename + "(DateTime,Open,High,Low,Close,TotalVolume)values('" + Datetime + "','" + Open + "','" + High + "','" + Low + "','" + Close + "','" + Totalvolume + "')";
                mys.Execute(strr);
                ltPI.Clear();
                return ("Write Data Ok");
            }
            catch(Exception ex)
            {
                return ("Data write error");
            }
        }


        /// <summary>
        /// Calendar write api
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="nameen"></param>
        /// <param name="namecn"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="contenten"></param>
        /// <param name="contentcn"></param>
        /// <returns></returns>
        [HttpGet]
        public string OnCalendarWrite(string datetime,string time,string nameen,string x,string y,string contenten,string contentcn,string sign)
        {
            if (sign != "mytone20050212")
            {
                return ("login error");
            }
            string stx = "insert into fcdata (datetime,time,nameen,contenten,contentcn,x,y)";
            
            try
            {
                stx=stx+"values('"+datetime+"','"+time+"','"+nameen+"','"+contenten+"','"+contentcn+"','"+x+"','"+y+"')";
                MySqlContext mys = new MySqlContext();
                mys.Execute(stx);


                return ("calendar write ok");
            }catch(Exception ex)
            {
                return ("Calendar write error");
            }
        }


        //fund value//////
        [HttpGet]
        public string OnFundValueWriteDay(string dt,string nw, string cnw, string cd, string dp,string ps,string tn)
        {
            string Pass = HttpUtility.UrlDecode(ps).ToString();
            if (Pass != "LJHnLKaJ_7868hkjhk9760gjk_96970HJHLKHKLHsdfs9879870")
            {
                return ("Error");
            }
            string Datetime = HttpUtility.UrlDecode(dt);
            float Nw = Convert.ToSingle( HttpUtility.UrlDecode(nw));
            float Cnw = Convert.ToSingle(HttpUtility.UrlDecode(cnw));
            string Code = HttpUtility.UrlDecode(cd);
            string Description = HttpUtility.UrlDecode(dp);
            string Tablename = HttpUtility.UrlDecode(tn);

            MySqlContext mys = new MySqlContext();
            string strr = "select * from " + Tablename + " where datetime='" + Datetime + "' and nw=" + Nw + " and cnw=" + Cnw + " ";
            List<Dictionary<string, string>> ltPI = new List<Dictionary<string, string>>();
            ltPI = mys.GetSqlDatas(strr);
            if (ltPI.Count > 0)
            {
                return ("Have");
            }
            //strr = "insert into " + Tablename + "(DateTime,Open,High,Low,Close,TotalVolume)values('" + Datetime + "','" + Open + "','" + High + "','" + Low + "','" + Close + "','" + Totalvolume + "')";
            //mys.Execute(strr);
            return ("Ok");
        }




        /// <summary>
        /// 
        /// cac md5 value
        /// 
        /// </summary>
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }



        public string OnTitleWrite()
        {
            return ("");
        }


    }
}