using ConsentManagement.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsentManagement.Models.UsersManagement;

namespace ConsentManagement.Services
{
    public class UsersServices: IUsersServices
    {
        public static string pLoginIp;
        private string sql;
        private DBUtility _db;
        SqlCommand com = new SqlCommand();
        DataTable dt;
        public UsersServices(DBUtility db)
        {
            _db = db;
        }
        public List<UserModel> getUserLists(string userId,string loginUser)
        {
            List<UserModel> ls = new List<UserModel>();
            sql = "select * from dbo.udf_cnms_get_user_detail('@p1')";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", userId);
            dt = _db.GetDataTable(com);
            int i = 1;
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var m = new UserModel();
                    m.ID = dr["id"].ToString();
                    m.Userno = Convert.ToInt32(dr["user_no"].ToString());
                    m.Username = dr["user_name"].ToString();
                    m.Role = dr["user_role"].ToString();
                    m.CreateDate = Convert.ToDateTime(dr["user_create"].ToString());
                    m.LastLogin = Convert.ToDateTime(dr["user_last_login"].ToString());
                    m.Status = dr["user_status"].ToString();
                    i++;
                    ls.Add(m);
                }
            }
            return ls;
        }
    }
    public interface IUsersServices
    {
        List<UserModel> getUserLists(string userId, string loginUser);
    }
}
