using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace wzhoo.Apps
{
    public class HttpOrder
    {
        //http api get  return string
        public string GetHttp(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Timeout = 20000;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader mystreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = mystreamReader.ReadToEnd();

            mystreamReader.Close();
            myResponseStream.Close();
            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            return retString;
        }

        
        //http api post  return string
        public string Post(string Url, string Data, string Referer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            //request.Referer = Referer;
            //byte[] bytes = Encoding.UTF8.GetBytes(Data);
            string str_postdata = JsonConvert.SerializeObject(new { tablename = "6a_day", datetime = "2020-4-1", open = "234.33", high = "89", low = "9.009", close = "99", pass = "LJHnLKaJ_7868hkjhk9760gjk_96970HJHLKHKLHsdfs9879870" });
            request.ContentType = "application/json;charset=UTF-8";
            //request.ContentLength = bytes.Length;
            //Stream myResponseStream = request.GetRequestStream();
            //myResponseStream.Write(bytes, 0, bytes.Length);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(str_postdata);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            //myResponseStream.Close();
            myStreamReader.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            return retString;
        }


        /*
         *  url:POST请求地址
         *  postData:json格式的请求报文,例如：{"key1":"value1","key2":"value2"}
        */
        public string PostUrl(string url, string postData)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 3000;    //设置请求超时时间，单位为毫秒
            req.ContentType = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

    }
}
