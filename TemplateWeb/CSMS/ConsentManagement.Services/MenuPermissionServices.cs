using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsentManagement;
using ConsentManagement.Models;

namespace ConsentManagement.Services
{
    public class MenuPermissionServices : IMenuPermissionServices
    {
        public static string pLoginIp;
        private string sql;
        private DBUtility _db;
        SqlCommand com = new SqlCommand();
        DataTable dt;
        public MenuPermissionServices(DBUtility db)
        {
            _db = db;
        }
        
        public ICollection<MenuPermissionModel> GetMenuPermission()
        {
            string sql;
            ICollection<MenuPermissionModel> ic = new List<MenuPermissionModel>();
            sql = "select * from [MVC].[ufn_GetMenuPermissionMvc]('ConsentManagement','soraya') ";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            dt = _db.GetDataTable(com);

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var m = new MenuPermissionModel();
                    m.ProgramCode = dr["ProgramCode"].ToString();
                    m.MenuDescription = dr["MenuDescription"].ToString();
                    //m.MenuDescription = Convert.ToDecimal(dr["SeqNo"]);
                    ic.Add(m);
                }
            }

            return ic;
        }
    }
    public interface IMenuPermissionServices
    {
        ICollection<MenuPermissionModel> GetMenuPermission();

    }
}
