using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LitJson;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace wzhoo.Apps
{
    public class CommonTool
    {


        /// <summary>
        /// 
        /// Get variety name from string vvn       updatetime 20201022 
        /// 
        /// </summary>
        /// <param name="vvn"></param>
        /// <returns></returns>
        public string GetVarietyLabel(string vvn)
        {
            string stx="";
            if (vvn.Contains("wheat"))
            {
                stx = stx + "zw|";
            }
            if (vvn.Contains("corn"))
            {
                stx = stx + "zc|";
            }
            if (vvn.Contains("soybean"))
            {
                stx = stx + "zs|";
            }
            if (vvn.Contains("crude oil") || vvn.Contains("crude") || vvn.Contains("oil"))
            {
                stx = stx + "cl|";
            }
            if (vvn.Contains("gold"))
            {
                stx = stx + "gc|";
            }
            if (vvn.Contains("dollar") || vvn.Contains("trillion"))
            {
                stx = stx + "dx|";
            }
            if (vvn.Contains("copper"))
            {
                stx = stx + "hg|";
            }
            if (vvn.Contains("euro"))
            {
                stx = stx + "6e|";
            }
            return (stx);
        }



        /// <summary>
        /// 
        /// cn to en translate     updatetime 20201022
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public string TranslateString(string Query)
        {
            // 原文
            string q = Query;
            // 源语言
            string from = "zh";
            // 目标语言
            string to = "en";
            // 改成您的APP ID
            string appId = "20180807000192443";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "ln48otJTGllzzroH2IB2";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            JsonData dejson = LitJson.JsonMapper.ToObject(retString);
            return(dejson[2][0]["dst"].ToString());
        }


        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            byte[] byteNew = md5.ComputeHash(byteOld);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }


}