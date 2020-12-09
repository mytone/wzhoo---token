using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wzhoo.Models;
using System.Web;


namespace wzhoo
{
    public interface IUserService
    {
        string IsValid(LoginRequestDTO req,  out Dictionary<string,string> userinfox );
        string AreValid(LoginRequestDTO req, out Dictionary<string, string> userinfox);
    }

    public class UserService : IUserService
    {
        // login deal
        public string IsValid(LoginRequestDTO req,out Dictionary<string,string> userinfox)
        {
      
            MySqlContext mys = new MySqlContext();
            List<Dictionary<string,string>> uss = new List<Dictionary<string,string>>();
            var email = HttpUtility.UrlDecode(req.Email).ToString();
            var password = HttpUtility.UrlDecode(req.Password).ToString();
            Dictionary<string, string> tt = new Dictionary<string, string>();

            //valid
            string strr = "select * from userinfo where email='"+email+"'  and password='"+password+"'";
            uss = mys.GetSqlDatas(strr);


            if (uss.Count > 0)
            {
                tt.Add("id", uss[0]["id"].ToString());
                tt.Add("email", uss[0]["email"].ToString());
                tt.Add("createtime",uss[0]["createtime"].ToString());
                userinfox = tt;
                return ("true");
            }
            else
            {
                userinfox = tt;
                return ("false");
            }

        }

        //new account register deal 
        public string AreValid(LoginRequestDTO req, out Dictionary<string, string> userinfox)
        {
            MySqlContext mys = new MySqlContext();
            List<Dictionary<string, string>> uss = new List<Dictionary<string, string>>();
            var email = HttpUtility.UrlDecode(req.Email).ToString();
            var password = HttpUtility.UrlDecode(req.Password).ToString();
            Dictionary<string, string> tt = new Dictionary<string, string>();

            string strr = "select * from userinfo where email='" + email + "'";
            uss = mys.GetSqlDatas(strr); 
            if (uss.Count > 0)
            {
                userinfox = tt;
                uss.Clear();
                return ("userexist");
            }           

            strr = "insert into userinfo (email,password,createtime)value('"+email+"','"+password+"','"+DateTime.Now.ToLocalTime().ToString()+"')";
            mys.Execute(strr);
            strr = "select * from userinfo where email='" + email + "'  and password='" + password + "'";
            uss = mys.GetSqlDatas(strr);
            if (uss.Count > 0)
            {
                tt.Add("id", uss[0]["id"].ToString());
                tt.Add("email", uss[0]["email"].ToString());
                tt.Add("createtime", uss[0]["createtime"].ToString());
                userinfox = tt;
                uss.Clear();
                return ("true");
            }
            else
            {
                userinfox = tt;
                uss.Clear();
                return ("false");
            }
            
        }




    }

}
