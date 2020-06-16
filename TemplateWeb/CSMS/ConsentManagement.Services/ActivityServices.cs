using ConsentManagement.Models.UsersManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentManagement.Services
{
    public class ActivityServices: IActivityServices
    {
        public static string pLoginIp;
        private string sql;
        private DBUtility _db;
        SqlCommand com = new SqlCommand();
        DataTable dt;
        public ActivityServices(DBUtility db)
        {
            _db = db;
        }
        public bool saveActivityLog(ActivityModel act)
        {
            sql = "dbo.udp_cnms_save_activity_log";
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = sql;
            //com.Parameters.AddWithValue("@puser_Id", act.UserId);
            com.Parameters.AddWithValue("@puser_name", act.Username);
            com.Parameters.AddWithValue("@pactivity", act.Activity);
            com.Parameters.AddWithValue("@premark", act.Remark);
            //com.Parameters.AddWithValue("@plog_date_time", act.LogDatetime);
            try {
                _db.GetDataTable(com);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
                

        }
    }
    public interface IActivityServices
    {
        bool saveActivityLog(ActivityModel act);
    }
}
