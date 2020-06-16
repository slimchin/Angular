//using Novell.Directory.Ldap;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ConsentManagement.Services
//{
//    public class AuthenticationService: IAuthenticationService
//    {

//        private DBUtility _db;
//        public AuthenticationService( DBUtility db)
//        {
//            _db = db;
//        }

//        public bool LdapAuthen(string loginDn, string password)
//        {
//            Boolean rs = false;
//            string ldapHost = "172.20.50.254";
//            int ldapPort = 389;
//            try
//            {
//                LdapConnection ldapConn = new LdapConnection();
//                ldapConn.Connect(ldapHost, ldapPort);
//                ldapConn.Bind(loginDn,password);
//                rs = true;
//                ldapConn.Disconnect();
//            }
//            catch
//            {
//                rs = false;
//            }
//            return rs;
//        }
//        public bool UserPermission(string userLogin)
//        {
//            var con = new SqlConnection();
//            var com = new SqlCommand();
//            con.Open();
//            com.Connection = con;

//            com.CommandText = " select * from dbo.udf_cnms_get_user_detail(@p1) ";
//            com.CommandType = CommandType.Text;
//            com.Parameters.AddWithValue("@p1", userLogin);
//            using (var re = com.ExecuteReader())
//            {
//                using (var dt = new DataTable())
//                {
//                    if (re == null)
//                    {
//                        return  false;
//                    }
//                    else if (!re.HasRows)
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//            }
//        }
//    }

//    public interface IAuthenticationService
//    {
//        bool LdapAuthen(string loginDn, string password);
//        bool UserPermission(string userLogin);
//        //    public string ConStr { get; }
//        //    CeoOfficeRptOpenAccountModel Getdata();
//    }
//}
