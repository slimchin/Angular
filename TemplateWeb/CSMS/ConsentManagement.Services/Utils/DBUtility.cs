using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace ConsentManagement.Services
{
    public class DBUtility
    {
        public static string pIp;
        public static string pDbName;
        public static int pConnectionTimeout;
        public static string pConstr;

        public string con { get; set; }

        public DBUtility()
        {

        }

        public DBUtility(string conString) {
            pConstr = conString;
        }

        public DBUtility(string ip,string DbName,int ConnectionTimeout, string conString)
        {
            pIp = ip;
            pDbName = DbName;
            pConnectionTimeout = ConnectionTimeout;
            //pConstr = "Data Source=" + pIp + ";Initial Catalog=" + pDbName + ";Persist Security Info=True;User ID=nbo;Password=nboP@$$W0RD;Connection Timeout=" + Convert.ToString(pConnectionTimeout);
            pConstr = conString;
        }
        public  DateTime GetServerdate()
        {
            DateTime valueReturn;
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(pConstr);
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            com.CommandText = "SELECT GETDATE()";
            com.CommandTimeout = pConnectionTimeout;
            com.Connection = con;
            con.Open();
            valueReturn = Convert.ToDateTime(com.ExecuteScalar());
            con.Close();
            if (com != null)
            {
                com.Dispose();
            }
            if (con != null)
            {
                con.Dispose();
            }
            return valueReturn;
        }
        public void DirectExecQuery(string sql)
        {
            SqlConnection con = new SqlConnection(pConstr);
            SqlCommand com = new SqlCommand();
            com.CommandText = sql;
            com.CommandTimeout = pConnectionTimeout;
            com.Connection = con;
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            if (com != null)
            {
                com.Dispose();
            }
            if (con!=null)
            {
                con.Dispose();
            }
        }
        public void DirectExecQuery(SqlCommand com)
        {
            SqlConnection con = new SqlConnection(pConstr);
            com.CommandTimeout = pConnectionTimeout;
            com.Connection = con;
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            if (com != null)
            {
                com.Dispose();
            }
            if (con != null)
            {
                con.Dispose();
            }
        }
        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConstr);
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.CommandTimeout = pConnectionTimeout;
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();
            if (dr != null)
            {
                dr.Dispose();
            }
            if (com != null)
            {
                com.Dispose();
            }
            if (con != null)
            {
                con.Dispose();
            }
            return dt;
        }
        public DataTable GetDataTable(SqlCommand com)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(pConstr);
            com.CommandTimeout = pConnectionTimeout;
            com.Connection = con;
            con.Open();
            dr = com.ExecuteReader();
            dt.Load(dr);
            con.Close();
            if (dr != null)
            {
                dr.Dispose();
            }
            if (com != null)
            {
                com.Dispose();
            }
            if (con != null)
            {
                con.Dispose();
            }
            return dt;
        }
    }
}