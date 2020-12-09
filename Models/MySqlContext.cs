using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.Cms;

namespace wzhoo.Models
{
    /// excute mysql class
    public class MySqlContext
    {
        public string connect_str;
        public MySqlContext()   //init
        {
            connect_str = "Data Source=localhost;User ID=root;Password=zc76122259000412; Database=tradedata; Allow User Variables=True; Charset=utf8;";
        }
        public void Execute(string cmd)   //insert
        {
            try
            {
                using (MySqlConnection SqlConnection = new MySqlConnection(connect_str))
                {
                    SqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(cmd, SqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    SqlConnection.Close();
                }
            }catch(Exception ex)
            {

            }

        }

        public List<Dictionary<string, string>> GetSqlDatas(string cmd)    //query
        {
            using (var SqlConnection = new MySqlConnection(connect_str))
            {
                SqlConnection.Open();
                List<Dictionary<string, string>> sqlDatas_list = new List<Dictionary<string, string>>();
                MySqlCommand mySqlCommand = new MySqlCommand(cmd, SqlConnection);
                MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    Dictionary<string, string> pairs = new Dictionary<string, string>();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        pairs[dataReader.GetName(i)] = dataReader[dataReader.GetName(i)].ToString();
                    }
                    sqlDatas_list.Add(pairs);
                }
                dataReader.Close();
                mySqlCommand.ExecuteNonQuery();
                SqlConnection.Close();
                return sqlDatas_list;
            }
        }
    }

    public class TableSituation
    {
        public string id { get; set; }
        public string table_name { get; set; }  
        public string rows { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string comment { get; set; }
    }

    public class VarietyTable
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string time { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string totalvolume { get; set; }
        public string eventz { get;set; }
    }


    public class VarietyGeneral
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string cost { get; set; }
        public string country { get; set; }

    }

    public class VarietyTableTick
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string price { get; set; }
        public string totalvolume { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        [Required(ErrorMessage ="must write!")]
        [StringLength(200,MinimumLength =5,ErrorMessage ="At least 5 letter,and up to 200 letters!")]
        public string content { get; set; }
        [Required(ErrorMessage = "must write!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "At least 3 letter,and up to 20 letters!!")]
        public string name { get; set; }
        [Required(ErrorMessage = "must write!")]
        [EmailAddress]
        [Display(Name ="Eamil Address")]
        public string email { get; set; }
        public string variety { get; set; }
        public string createtime { get; set; }
    }

    // userinfo ///////////////////////////////////
    public class UserInfo
    {
        public string id { get; set; }
        [Required(ErrorMessage = "must write!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "At least 3 letter,and up to 20 letters!!")]
        public string name { get; set; }
        [Required(ErrorMessage = "must write!")]
        [EmailAddress]
        [Display(Name = "Eamil Address")]
        public string email { get; set; }
        [Required(ErrorMessage = "must write!")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "At least 3 letter,and up to 20 letters!!")]
        [Display(Name = "Password")]
        public string password { get; set; }
        public string createtime { get; set; }
    }


    public class UserList
    {
        public string id { get; set; }
        public string content { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string variety { get; set; }
        public string createtime { get; set; }
    }

    public class IndexMenu
    {
        public string id { get; set; }
        public string name { get; set; }
        public string comment { get; set; }
        public string createtime { get; set; }
        public string link { get; set; }
        public string level { get; set; }
        public string dt { get; set; }
    }

    public class VarietyDayListAnaylsis
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string direction { get; set; }
        public int upwardshadow { get; set; }
        public int downshadow { get; set; }
        public int entity { get; set; }
        public int range { get; set; }
        public int volume { get; set; }
        public int openV { get; set; }
        public int closeV { get; set; }
    }

    //非农数据
    public class NfpList
    {
        public int id { get; set; }
        public string datetime { get; set; }
        public int cvalue { get; set; }
        public int prvalue { get; set; }
    }


    public class TableFinAnaylsis
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string currency { get; set; }
        public string eventz { get; set; }
        public string actual { get; set; }
        public string forecast { get; set; }
    }

    public class TableFund
    {
        public string name { get; set; }
        public string code { get; set; }
        public string createtime { get; set; }
        public double scale { get; set; }
        /*public float oneyearrr { get; set; }
        public float oneyearv { get; set; }
        public float oneyearsr { get; set; }
        public float oneyearmd { get; set; }
        public float threeyearrr { get; set; }
        public float threeyearv { get; set; }
        public float threeyearsr { get; set;｝
       
        public float fiveyearrr { get; set; }
        public float fiveyearv { get; set; }
        public float fiveyearsr { get; set; }
        public float fiveyearmd { get; set; }
        public float esyearrr { get; set; }
        public float esyearv { get; set; }
        public float esyearsr { get; set; }
        public float esyearmd { get; set; }*/
        public string esyearrr { get; set; }
        public string countcode { get; set; }
        public string countcode_j { get; set; }
        public string score { get; set; }
    }

    public class TableMoneyFlowAll
    {
        public string id { get; set; }
        public string status { get; set; }
        public string platform { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string prerateyear { get; set; }
        public string planday { get; set; }
        public string prerate { get; set; }
        public string cost { get; set; }
        public string returnmoney{ get; set; }
        public string buydatetime { get; set; }
        public string planreturndatetime { get; set; }
        public string description { get; set; }
        public string returndatetime { get; set; }
        public string acreturn { get; set; }
        public string description_f { get; set; }
    }

    public class TableTwoVarietyAnaylsis
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string totalvolume { get; set; }
        public string datetime2 { get; set; }
        public string open2 { get; set; }
        public string high2 { get; set; }
        public string low2 { get; set; }
        public string close2 { get; set; }
        public string totalvolume2 { get; set; }
    }

    public class TableTitle
    {
        public string id { get; set; }
        public string title { get; set; }
        public string contenten { get; set; }
        public string contentcn { get; set; }
        public string userid { get; set; }
        public string datetime { get; set; }
        public string label { get; set; }
        public string f { get; set; }
        
    }


    public class TableCalendar
    {
        public string id { get; set; }
        public string nameen { get; set; }
        public string namecn { get; set; }
        public string contenten { get; set; }
        public string contentcn { get; set; }
        public string y { get; set; }
        public string x { get; set; }
        public string label { get; set; }
        public string datetime { get; set; }
        public string time { get; set; }
    }

    public class NewTableCalendar
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string time { get; set; }
        public string currency { get; set; }
        public string eventz { get; set; }
        public string actual { get; set; }
        public string forecast { get; set; }
        public string previous { get; set; }
    }


    public class TableCoronavirous
    {
        public string id { get; set; }
        public string datetime { get; set; }
        public string time { get; set; }
        public string area { get; set; }
        public string totalcases { get; set; }
        public string newcases { get; set; }
        public string totaldeaths { get; set; }
        public string newdeaths { get; set; }
        public string totalrecovered { get; set; }
        public string activecases { get; set; }
        public string seriouscritical { get; set; }
    }

    

}
