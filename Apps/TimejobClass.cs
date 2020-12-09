using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Net;
using System.IO;
using HtmlAgilityPack;

using Pomelo.AspNetCore.TimedJob;
using System.Threading;
using wzhoo.Models;
using wzhoo.Apps;
using System.Web;
using LitJson;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace wzhoo.Apps
{
    public class TimejobClass : Job
    {

        public static int pt = 0;    //update time 2020-10-22


        /// <summary>
        /// time deal 
        /// </summary>
        [Invoke(Begin = "2020-08-30 7:01:00", Interval = 1000*60*5, SkipWhileExecuting = true)]         //10minute pre  
        public void DoTask()
        {
            //Method1();
            return;
        }


        public void Method1()
        {
            
            string pyesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");    //   date yesterday 
            string pcurdate = DateTime.Now.ToString("yyyy-MM-dd");                  //   date today

            ThreadPool.SetMaxThreads(5, 5);  
            if (pt % 48 == 0)   // 240minute pre    ///4hour
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(VarietyDayProcess),"");
            }
            
            if (pt % 6 == 0)    //30minute pre
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(InfointimeCalendarProcess),"");       //calendar get  1  eia nfp
                ThreadPool.QueueUserWorkItem(new WaitCallback(Mql5CalendarProcess), "");            //calendar get  2
                ThreadPool.QueueUserWorkItem(new WaitCallback(NewsProcess), "");
            }
            pt++;

        }



        /// <summary>
        /// 
        /// dedicate data level Day  sina finance site      ////////////////////////////////   2020-10-9 update
        /// 
        /// </summary>
        /// <param name="varl"></param>
        private void VarietyDayProcess(object varl)
        {
            snfdp("cl");
            Thread.Sleep(1500);
            snfdp("gc");
            Thread.Sleep(1500);
            snfdp("hg");
            Thread.Sleep(1500);
            snfdp("hsi");
            Thread.Sleep(1500);
            snfdp("bp");
            Thread.Sleep(1500);
            snfdp("ec");
            Thread.Sleep(1500);
            snfdp("cha50cfd");
            Thread.Sleep(1500);
            snfdp("w");
            Thread.Sleep(1500);
            snfdp("c");
            Thread.Sleep(1500);
            snfdp("s");
            Thread.Sleep(1500);
            ifdp("es");
            Thread.Sleep(1500);
            ifdp("dx"); //小写
            Thread.Sleep(1500);
            ifdp("ct"); //小写
            Thread.Sleep(1500);
            ifdp("nq");

        }
        /// <summary>
        /// 
        ///  dedicate data   level Day    --sina NYME finnace site     2020-10-7 update...
        /// 
        /// </summary>
        public void snfdp(string varl)
        {
            string ptime;
            string popen;
            string pclose;
            string phigh;
            string plow;
            string pvolume;            
            string responsetext;
            string qht="";
            string pyesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");    //   date yesterday 
            string pcurdate = DateTime.Now.ToString("yyyy-MM-dd");                  //   date today
            string variety="";

            string purl = "https://vip.stock.finance.sina.com.cn/q/view/vFutures_History.php?jys=NYME&pz="+varl+"&hy=&breed="+varl+"&type=global&start=" + pyesterday + "&end=" + pcurdate;
            try
            {
                HttpOrder hto = new HttpOrder();
                responsetext = hto.GetHttp(purl);
                var htmDoc = new HtmlDocument();
                htmDoc.LoadHtml(responsetext);
                qht = "//div[@class='historyList']//table//tr[2]//td[1]//div";
                ptime = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//div[@class='historyList']//table//tr[2]//td[2]//div";
                pclose = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//div[@class='historyList']//table//tr[2]//td[3]//div";
                popen = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//div[@class='historyList']//table//tr[2]//td[4]//div";
                phigh = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//div[@class='historyList']//table//tr[2]//td[5]//div";
                plow = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//div[@class='historyList']//table//tr[2]//td[6]//div";
                pvolume = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;

                if (varl.ToString() == "hg")
                {
                    pclose = (Convert.ToSingle(pclose) / 100).ToString("#0.0000");
                    popen = (Convert.ToSingle(popen) / 100).ToString("#0.0000");
                    phigh = (Convert.ToSingle(phigh) / 100).ToString("#0.0000");
                    plow = (Convert.ToSingle(plow) / 100).ToString("#0.0000");
                }

                switch (varl)
                {
                    case "ec":
                        variety = "6e";
                        break;
                    case "bp":
                        variety = "6b";
                        break;
                    case "cha50cfd":
                        variety = "a50";
                        break;
                    case "c":
                        variety = "zc";
                        break;
                    case "s":
                        variety = "zs";
                        break;
                    case "w":
                        variety = "zw";
                        break;
                    default:
                        variety = varl.ToString();
                        break;
                }

                MySqlContext mys = new MySqlContext();
                List<Dictionary<string, string>> vlist = new List<Dictionary<string, string>>();
                vlist = mys.GetSqlDatas(" select * from " + variety + "_day where datetime='" + ptime + " 00:00:00' ");
                if (vlist.Count <= 0)
                {
                    mys.Execute(" insert into " + variety + "_day(DateTime,Open,High,Low,Close,TotalVolume)values('" + ptime + "','" + popen + "','" + phigh + "','" + plow + "','" + pclose + "','" + pvolume + "') ");   //insert
                    mys.Execute(" update indexmenu  set createtime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where name='" + variety.ToUpper() + "' and level='Day' ");
                }               
            }
            catch (Exception ex)
            {                
            }
            return;
        }

               
        /// <summary>
        /// 
        ///  dedicate data programe  level Day    --www.infointime.cn  EIA finnace site  ////   update  2020-10-11
        /// 
        /// </summary> 
        private void InfointimeCalendarProcess(object varl)
        {
            EIA();
            Thread.Sleep(1500);
            NFP();
        }


        /// <summary>
        /// 
        ///  dedicate data programe  level Day   EIA  ////   update  2020-10-11
        /// 
        /// </summary>        
        public void EIA()
        {
            string datetime;
            string x;
            string y;
            string contentcn;
            //string contenten;
            string nameen = "EIA";
            string responsetext;
            string qht;
            string purl = "http://www.infointime.cn/keywords/oil-eia-eiashuju.htm";

            try
            {
                HttpOrder hto = new HttpOrder();
                responsetext = hto.GetHttp(purl);
                var htmDoc = new HtmlDocument();
                htmDoc.LoadHtml(responsetext);
                qht = "//table[2]//tr[2]//table[2]//table[2]//tr[2]//td[1]//font";
                datetime = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//table[2]//tr[2]//table[2]//table[2]//tr[2]//td[2]//font";
                contentcn = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//table[2]//tr[2]//table[2]//table[2]//tr[2]//td[3]//font";
                x = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//table[2]//tr[2]//table[2]//table[2]//tr[2]//td[5]//font";
                y = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
            
                if (x == "-" || x == null||x=="")
                {
                    return;
                }
                else
                {  
                    MySqlContext mys = new MySqlContext();
                    List<Dictionary<string, string>> vlist = new List<Dictionary<string, string>>();
                    vlist = mys.GetSqlDatas("select id,nameen,datetime from fcdata where nameen='"+nameen+"' and datetime='" + datetime + "'  ");
                    if (vlist.Count <= 0)
                    {
                        mys.Execute(" insert into fcdata(nameen,datetime,x,y,contentcn)values('" + nameen + "','" + datetime + "','" + x + "','" + y + "','"+contentcn+"') ");       //insert
                        //mys.Execute(" update indexmenu  set  createtime='" + datetime + "' and data='" + x + "' where name='EIA' ");
                    }
                    else
                    {
                        mys.Execute(" update fcdata set x='" + x + "',y='" + y + "',contentcn='" + contentcn + "' where nameen='" + nameen + "' and datetime='" + datetime + "' ");
                    }
                    return;
                }            
            }
            catch (Exception ex)
            {
               
            }             
        }


        /// <summary>
        /// 
        ///  dedicate data programe  level Day   NFP  ////   update  2020-10-11
        /// 
        /// </summary>        
        public void NFP()
        {
            string datetime;
            string x;
            string y;
            string contentcn;
            //string contenten;
            string nameen = "NFP";
            string responsetext;
            string qht;
            string purl = "http://www.infointime.cn/keywords/gold-feinong.htm";

            try
            {
                HttpOrder hto = new HttpOrder();
                responsetext = hto.GetHttp(purl);
                var htmDoc = new HtmlDocument();
                htmDoc.LoadHtml(responsetext);
                qht = "//table[2]//tr[2]//div[2]//table[1]//table[3]//tr[2]//td[1]//font";
                datetime = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Split(",")[0];
                qht = "//table[2]//tr[2]//div[2]//table[1]//table[3]//tr[2]//td[2]//font";
                contentcn = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//table[2]//tr[2]//div[2]//table[1]//table[3]//tr[2]//td[3]//font";
                x = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                qht = "//table[2]//tr[2]//div[2]//table[1]//table[3]//tr[2]//td[5]//font";
                y = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;

                if (x == "-" || x == null || x == "")
                {
                    return;
                }
                else
                {
                    MySqlContext mys = new MySqlContext();
                    List<Dictionary<string, string>> vlist = new List<Dictionary<string, string>>();
                    vlist = mys.GetSqlDatas("select id,nameen,datetime from fcdata where nameen='"+nameen+"' and datetime='" + datetime + "'  ");
                    if (vlist.Count <= 0)
                    {
                        mys.Execute(" insert into fcdata(nameen,datetime,x,y,contentcn)values('" + nameen + "','" + datetime + "','" + x + "','" + y + "','" + contentcn + "') ");       //insert
                        //mys.Execute(" update indexmenu  set  createtime='" + datetime + "' and data='" + x + "' where name='EIA' ");
                    }
                    else
                    {
                        mys.Execute(" update fcdata set x='" + x + "',y='" + y + "',contentcn='" + contentcn + "' where nameen='" + nameen + "' and datetime='" + datetime + "' ");
                    }
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }




        ///  <summary>
        /// 
        ///  dedicate data   level Day    --yahoo finnace site     2020-10-14 update...  
        ///  
        ///  </summary>
        public void ifdp(string varl)
        {
            string ptime;
            string popen;
            string pclose;
            string phigh;
            string plow;
            string pvolume;

            string ptime2;
            string popen2;
            string pclose2;
            string phigh2;
            string plow2;
            string pvolume2;

            string ptime3;
            string popen3;
            string pclose3;
            string phigh3;
            string plow3;
            string pvolume3;

            string responsetext;
            string qht = "";
            string pyesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");    //   date yesterday 
            string pcurdate = DateTime.Now.ToString("yyyy-MM-dd");                  //   date today
            string variety = varl;

            string purl = "https://finance.yahoo.com/quote/"+varl+"%3DF/history?p="+varl+"%3DF";
            try
            {                
                HttpOrder hto = new HttpOrder();
                responsetext = hto.GetHttp(purl);
                var htmDoc = new HtmlDocument();
                htmDoc.LoadHtml(responsetext);

                MySqlContext mys = new MySqlContext();
                List<Dictionary<string, string>> vlist = new List<Dictionary<string, string>>();

                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[1]//td[1]";
                ptime = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                string yyyy = ptime.Split(",")[1].ToString();
                string dd = ptime.Split(",")[0].Split(" ")[1].ToString();
                string mmtemp= ptime.Split(",")[0].Split(" ")[0].ToString();
                string mm = this.MonthConvertString(mmtemp);
                ptime = yyyy + "-" + mm + "-" + dd;  
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[1]//td[2]";
                popen = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",","");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[1]//td[3]";
                phigh = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",","");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[1]//td[4]";
                plow = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",","");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[1]//td[5]";
                pclose = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",","");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[1]//td[7]";
                pvolume = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",","");
                vlist = mys.GetSqlDatas(" select * from " + variety + "_day where DateTime='" + ptime + " 00:00:00' ");
                if ((vlist.Count <= 0) && (popen != "-") && (popen != "") && (popen != null))
                {
                    mys.Execute(" insert into " + variety + "_day(DateTime,Open,High,Low,Close,TotalVolume)values('" + ptime + "','" + popen + "','" + phigh + "','" + plow + "','" + pclose + "','" + pvolume + "') ");   //insert
                    mys.Execute(" update indexmenu  set createtime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where name='" + variety.ToUpper() + "' and level='Day' ");
                }
                else if ((popen != "-") && (popen != "") && (popen != null))
                {
                    mys.Execute(" update " + variety + "_day  set Open='" + popen + "',High='" + phigh + "',Low='" + plow + "',Close='" + pclose + "',TotalVolume='" + pvolume + "' where DateTime='" + ptime + " 00:00:00' ");   //insert
                }


                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[2]//td[1]";
                ptime2 = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                yyyy = ptime2.Split(",")[1].ToString();
                dd = ptime2.Split(",")[0].Split(" ")[1].ToString();
                mmtemp = ptime2.Split(",")[0].Split(" ")[0].ToString();
                mm = this.MonthConvertString(mmtemp);
                ptime2 = yyyy + "-" + mm + "-" + dd;
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[2]//td[2]";
                popen2 = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",", "");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[2]//td[3]";
                phigh2 = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",", "");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[2]//td[4]";
                plow2 = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",", "");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[2]//td[5]";
                pclose2 = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",", "");
                qht = "//div[@id='Col1-1-HistoricalDataTable-Proxy']//table[1]//tbody[1]//tr[2]//td[7]";
                pvolume2 = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText.Replace(",","");
                if ((popen2 != "-") && (popen2 != "") && (popen2 != null))
                {
                    mys.Execute(" update " + variety + "_day  set Open='" + popen2 + "',High='" + phigh2 + "',Low='" + plow2 + "',Close='" + pclose2 + "',TotalVolume='" + pvolume2 + "' where DateTime='" + ptime2 + " 00:00:00' ");   //
                }

            }
            catch (Exception ex)
            {
             }
            return;
        }

        // inner function  Oct to 11 ej.  update time: 2020-10-4
        public string MonthConvertString(string varl)
        {
            string mmtemp = varl;
            string mm="";
            switch (mmtemp)
            {
                case "Jan":
                    mm = "1";
                    break;
                case "Feb":
                    mm = "2";
                    break;
                case "Mar":
                    mm = "3";
                    break;
                case "Apr":
                    mm = "4";
                    break;
                case "May":
                    mm = "5";
                    break;
                case "Jun":
                    mm = "6";
                    break;
                case "Jul":
                    mm = "7";
                    break;
                case "Aug":
                    mm = "8";
                    break;
                case "Sept":
                    mm = "9";
                    break;
                case "Oct":
                    mm = "10";
                    break;
                case "Nov":
                    mm = "11";
                    break;
                case "Dec":
                    mm = "12";
                    break;
            }
            return mm;
        }





        /// <summary>
        /// 
        ///  dedicate data programe  level Day    --www.mql5.cn  finnace site  ////   update  2020-10-19
        /// 
        /// </summary> 
        private void Mql5CalendarProcess(object varl)
        {
            mcp();
            Thread.Sleep(1500);
        }
        /// <summary>
        /// 
        /// www.myl5.com/en/economic-calendar  collect data   insert  update   update time:2020-10-19
        /// 
        /// </summary>
        public void mcp()
        {
            string pdatetime="";
            string ptime="";
            string pcurrency="";
            string pevent="";
            string pactual="";
            string pforecast="";
            string pprevious="";
            string responsetext="";
            string qht = "";
            string tmpt="";
            string purl = "https://www.mql5.com/en/economic-calendar";

            try
            {
                HttpOrder hto = new HttpOrder();
                responsetext = hto.GetHttp(purl);
                var htmDoc = new HtmlDocument();
                htmDoc.LoadHtml(responsetext);
                MySqlContext mys = new MySqlContext();
                for (int i = 1; i < 800; i++)
                {
                    qht = "//div[@id='economicCalendarTable']//div[" + i.ToString() + "]";
                    tmpt = htmDoc.DocumentNode.SelectSingleNode(qht).InnerText;
                    if (tmpt == null || tmpt == "")
                    {
                        break;
                    }
                    else
                    {
                        var nodex = tmpt.Split(",");
                        int b = nodex.Length;
                        pdatetime = nodex[0].Split(" ")[0].Trim();                            
                        ptime     = nodex[0].Split(" ")[1].Trim();
                        pcurrency = nodex[1].Trim();
                        pevent    = nodex[2].Trim();
                        for (var j = 3; j < b; j++)   // last 3 programe
                        {
                            switch ( nodex[j].Split(":")[0].Trim() )
                            {
                                case "Actual":
                                    pactual = nodex[j].Split(":")[1].Trim();
                                    break;
                                case "Forecast":
                                    pforecast = nodex[j].Split(":")[1].Trim();
                                    break;
                                case "Previous":
                                    pprevious = nodex[j].Split(":")[1].Trim();
                                    break;
                            }                            
                        }                   
                        List<Dictionary<string, string>> vlist = new List<Dictionary<string, string>>();
                        vlist = mys.GetSqlDatas(" select * from calendar where datetime='"+pdatetime+"' and currency='"+pcurrency+"' and event='"+pevent+"' ");
                        if (vlist.Count <= 0)
                        {
                            mys.Execute(" insert into calendar(datetime,time,currency,event,actual,forecast,previous)values('" + pdatetime +"','" + ptime+"','" +pcurrency +"','" + pevent + "' ,'" + pactual + "','" + pforecast+ "','"+pprevious+"') ");       //insert  
                        }
                        else
                        {
                            mys.Execute(" update calendar set actual='"+pactual +"' , forecast='"+pforecast +"' , previous='"+pprevious+"' , time='"+ptime+ "'  where datetime='" + pdatetime + "' and currency='" + pcurrency + "' and event='" + pevent + "' ");
                        }
                        vlist.Clear();
                    }
                    pactual = "";
                    pforecast = "";
                    pprevious = "";
                    pdatetime = "";
                    pcurrency = "";
                    pevent = "";
                    tmpt = "";
                }

            }
            catch (Exception ex)
            {
            }           
            return;
        }




        /// <summary>
        /// 
        /// fast news collect      updatetime 20201022
        /// 
        /// </summary>
        /// <param name="varl"></param>
        private void NewsProcess(object varl)
        {          
            string responsetext;
            string qt;
            string vvcn;
            string vven;
            string labelz;
            string datetimez;
            HtmlNode mt;
            string xt;
            string purl = "https://www.jin10.com";
            List<Dictionary<string, string>> ltpi = new List<Dictionary<string, string>>();

            try
            {
                HttpOrder hto = new HttpOrder();
                responsetext = hto.GetHttp(purl);
                var htmdoc = new HtmlDocument();
                htmdoc.LoadHtml(responsetext);
                CommonTool ct = new CommonTool();
                MySqlContext mys = new MySqlContext();

                for (var i = 30; i > 1; i--)
                {
                    qt = "//div[@id='jin_flash_list']//div[" + Convert.ToString(i) + "]//div[@class='right-content']//div[1]";
                    mt = htmdoc.DocumentNode.SelectSingleNode(qt);
                    if (mt == null || mt.InnerText.Length < 20)
                    {
                        continue;
                    }
                    else
                    {
                        vvcn = mt.InnerText;
                        qt = "";
                        vvcn = vvcn.Replace("详情<!---->", "");        ////filtration///////
                        vvcn = vvcn.Replace("<!---->", "");
                        vvcn = vvcn.Replace("欢迎点击查看", "");
                        if (vvcn.Contains("】"))
                        {
                            vvcn = vvcn.Split("】")[1];
                        }
                        string regexstr = string.Empty;
                        if (Regex.IsMatch(vvcn, "/北向|证监|银监|财政部|部委|全国|京|国务院|卫健|中共|中方|中央|上期|推荐阅读|中金|工信|中国|上海|发改委|金十|大商|点击|深交|民政部/g"))
                        {
                            //Console.WriteLine(vvcn);
                        }
                        else
                        {
                            vven = ct.TranslateString(vvcn);
                            vven = HttpUtility.UrlDecode(vven).Trim();
                            vvcn = HttpUtility.UrlDecode(vvcn).Trim();
                            labelz = ct.GetVarietyLabel(vven);
                            datetimez = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
                            Dictionary<string, string> vv = new Dictionary<string, string>();
                            vv["contenten"] = vven;
                            vv["contentcn"] = vvcn;
                            vv["label"] = labelz;
                            vv["datetime"] = datetimez;
                            ltpi.Add(vv);
                            Console.WriteLine(vvcn); Console.WriteLine(vven);
                        }
                        Thread.Sleep(2500);
                    }
                }
                List<Dictionary<string, string>> kk = new List<Dictionary<string, string>>();
                for (var j = 0; j < ltpi.Count; j++)
                {
                    kk = mys.GetSqlDatas(" select contentcn from title where contentcn='" + ltpi[j]["contentcn"] + "' ");
                    if (kk.Count < 1)
                    {
                        xt = "insert into title(contenten,contentcn,datetime,f,label,userid)value('<p>" + ltpi[j]["contenten"] + "</p>','<p>" + ltpi[j]["contentcn"] + "</p>','" + ltpi[j]["datetime"] + "','b','" + ltpi[j]["label"] + "',1)";
                        mys.Execute(xt);
                    }
                }
            }
            catch(Exception ex)
            {
            }       
            
        }

    }
}
