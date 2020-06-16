using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
public enum LogInStatus { Successed, Failed, Expired, Suspended, ForceChgPassword, IncorrectPasswordOver, NonActive, NotPermissionToUse };
public enum ChangePasswordStatus { Successed, IncorrectPassword, IncorrectUserId, NewPassNotEqual, SamePreviousPassword };

namespace ConsentManagement
{
    public class UserManagement : IDisposable
    {
        public static string pLoginIp;
        public static string pUserId;
        public static string pUserName;
        public static string pProgramCode;
        public string ErrMsg;
        private string pPassword;
        bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public UserManagement(string LoginIp, string UserId, string PassWord, string ProgramCode)
        {
            pLoginIp = LoginIp;
            pUserId = UserId;
            pPassword = PassWord;
            pProgramCode = ProgramCode;
        }
        public UserManagement()
        {

        }
        private string EncryptPassword(string argUser, string argPass)
        {
            string w_char = "", result = "";
            byte[] strPass;
            int i, ilenUid;
            if (argPass.Trim() == "")
            {
                return "";
            }
            ilenUid = argUser.Length;
            strPass = Encoding.ASCII.GetBytes(argPass);

            for (i = strPass.Length; i > 0; i--)
            {
                w_char += Convert.ToString(strPass[i - 1] + ilenUid);
            }
            result = w_char + Convert.ToString(Encoding.ASCII.GetBytes(argPass.Length.ToString().Substring(0, 1))[0]);
            return result;
        }

        //อ่านสิทธิเมนู
        //public static bool ReadProgramCode(string argUser, string argProCode)
        //{
        //    bool result = false;
        //    string sql;
        //    sql = "select UserProgramCode from BACKOFFICEUSERCENTER where USERID  = @p1";
        //    DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
        //    SqlCommand com = new SqlCommand();
        //    DataTable dt;
        //    com.CommandType = CommandType.Text;
        //    com.CommandText = sql;
        //    com.Parameters.AddWithValue("@p1", argUser);
        //    dt = db.GetDataTable(com);
        //    if (dt!=null)
        //    {
        //        string text = dt.Rows[0].ToString();
        //        string[] ProCd = text.Split('|');
        //        foreach (string proCd in ProCd)
        //        {
        //            if(argProCode== proCd)
        //            {
        //                result = true;
        //                break;
        //            }else
        //            {
        //                result = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result = false;
        //    }
        //    com.Parameters.Clear();
        //    com.Dispose();
        //    dt.Dispose();
        //    return result;
        //}


        public static bool ReadMenuPermission(string argUser, string argMenuCode)
        {
            bool result = false;
            string sql;
            sql = "select UserID,ProgramCode,MenuCode,MenuPermission from UserPermission where ProgramCode  = @p1 and MenuCode = @p2 and UserID = @p3";
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pProgramCode);
            com.Parameters.AddWithValue("@p2", argMenuCode);
            com.Parameters.AddWithValue("@p3", argUser);
            dt = db.GetDataTable(com);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    result = true;
                else
                    result = false;
            }
            else
            {
                result = false;
            }
            com.Parameters.Clear();
            com.Dispose();
            dt.Dispose();
            return result;
        }
        public static bool ReadMenuPermission2(string argUser, string functionName)
        {
            bool result = false;
            string sql;
            sql = "select * from dbo.udf_cnms_web_get_user_access_permission(@puser_name,@pfunction_name)";
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.Clear();
            com.Parameters.AddWithValue("@puser_name", argUser);
            com.Parameters.AddWithValue("@pfunction_name", functionName);
            dt = db.GetDataTable(com);
            if (dt != null)
            {
                if (dt.Rows[0]["can_access"].ToString()=="Y")
                    result = true;
                else
                    result = false;
            }
            else
            {
                result = false;
            }
            com.Parameters.Clear();
            com.Dispose();
            dt.Dispose();
            return result;
        }

        public static bool ReadMenuPermissionForMKTI(string argUser)
        {
            bool result = false;
            string sql;
            // sql = "select UserID,ProgramCode,MenuCode,MenuPermission from UserPermission where ProgramCode  = @p1 and MenuCode = @p2 and UserID = @p3";
            sql = "SELECT UserID,UserName,UserPass,UserInUse,UserLevel,UserStartDate,UserIncorrectPass,CountChgPass,UserLastLogin,UserProgramCode FROM SsTraderUser WHERE UserInUse = 'Y' and UserID = @p1";
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", argUser);
            dt = db.GetDataTable(com);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    result = true;
                else
                    result = false;
            }
            else
            {
                result = false;
            }
            com.Parameters.Clear();
            com.Dispose();
            dt.Dispose();
            return result;
        }



        public static List<string> ReadMenuPermission(string argUser)
        {
            string sql;
            List<string> tempList = new List<string>();
            sql = "select MenuCode from UserPermission where ProgramCode  = @p1 and UserID = @p2";
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pProgramCode);
            com.Parameters.AddWithValue("@p2", argUser);
            dt = db.GetDataTable(com);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tempList.Add(dt.Rows[i]["MenuCode"].ToString());
                }
            }
            com.Parameters.Clear();
            com.Dispose();
            return tempList;
        }

        private int GetPolicy(string PolicyId)
        {
            int PolicyDays = 0;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "pGetPolicy";
            com.Parameters.AddWithValue("@pPolicyCode", PolicyId);
            dt = db.GetDataTable(com);
            if (dt != null)
            {
                PolicyDays = Convert.ToInt32(dt.Rows[0]["POLICYVALUE"]);
            }
            else
            {
                PolicyDays = 0;
            }
            com.Parameters.Clear();
            com.Dispose();
            dt.Dispose();
            return PolicyDays;
        }
        private void UpdateIncorrectPass(string user, int incorrectValue)
        {
            string sql;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            sql = "UPDATE BACKOFFICEUSERCENTER SET UserIncorrectPass = " + incorrectValue + " WHERE USERID='" + user + "'";
            db.DirectExecQuery(sql);
        }

        private void updateUserChangePass(string user, string NewPassword)
        {
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            string sql = "UPDATE BACKOFFICEUSERCENTER SET UserPass=@p1 ,UserStartDate = convert(varchar,getdate(),112)  WHERE USERID = @p2";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.Clear();
            com.Parameters.AddWithValue("@p1", NewPassword);
            com.Parameters.AddWithValue("@p2", pUserId);
            db.DirectExecQuery(com);
            com.Dispose();


            sql = "UPDATE SsTraderUser SET UserPass=@p1 ,UserStartDate = convert(varchar,getdate(),112)  WHERE USERID = @p2";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.Clear();
            com.Parameters.AddWithValue("@p1", NewPassword);
            com.Parameters.AddWithValue("@p2", pUserId);
            db.DirectExecQuery(com);
            com.Dispose();


        }
        private void updateCountUserChangePass(string user)
        {
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            string sql = "UPDATE BACKOFFICEUSERCENTER SET CountChgPass=CountChgPass+1  WHERE USERID = @p1";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pUserId);
            db.DirectExecQuery(com);
            com.Dispose();
        }


        private void instLogChangePass(string NewPassword)
        {
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DateTime tempDateTime = db.GetServerdate();
            string sql = "INSERT INTO LOGUSERCHGPASS(UserID,UserPassword,UserDateChange,UserTimeChange) VALUES(@p1,@p2,@p3,@p4)";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pUserId);
            com.Parameters.AddWithValue("@p2", NewPassword);
            com.Parameters.AddWithValue("@p3", tempDateTime.ToString("yyyyMMdd", new CultureInfo("en-US")));
            com.Parameters.AddWithValue("@p4", tempDateTime.ToString("hh:mm:ss", new CultureInfo("en-US")));
            db.DirectExecQuery(com);
            com.Dispose();
        }
        private bool IsNewPassNotSamePrevious(string NewPass)
        {
            int policyLastChg; int i; bool chkOld;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            string sql = "SELECT USERPASSWORD FROM LOGUSERCHGPASS WHERE USERID = @p1 ORDER BY USERDATECHANGE DESC, USERTIMECHANGE DESC";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pUserId);
            dt = db.GetDataTable(com);
            com.Parameters.Clear();
            policyLastChg = GetPolicy("OP");
            chkOld = true;
            i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (i > policyLastChg || chkOld != true)
                {
                    break;
                }
                else
                {
                    if (dr["USERPASSWORD"].Equals(NewPass))
                    {
                        chkOld = false;
                    }
                }
                i += 1;
            }
            com.Dispose();
            dt.Dispose();
            return chkOld;
        }

        public ChangePasswordStatus ChangePassword(string newPass1, string newPass2)
        {
            string encryptPass;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            bool TempCheckMKT = false; bool TempCheckBO = false;

            string sql = "SELECT UserID,UserPass,UserInUse,UserLevel,UserStartDate,UserIncorrectPass,CountChgPass,UserLastLogin,UserProgramCode FROM BACKOFFICEUSERCENTER WHERE USERID = @p1";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pUserId);
            dt = db.GetDataTable(com);
            com.Parameters.Clear();


            if (dt.Rows.Count.Equals(0))
            {
                TempCheckBO = false;

                sql = "SELECT UserID,UserPass,UserInUse,UserLevel,UserStartDate,UserIncorrectPass,CountChgPass,UserLastLogin,UserProgramCode FROM SsTraderUser WHERE USERID = @p1";
                com.CommandType = CommandType.Text;
                com.CommandText = sql;
                com.Parameters.AddWithValue("@p1", pUserId);
                dt = db.GetDataTable(com);
                com.Parameters.Clear();

                if (dt.Rows.Count.Equals(0))
                {
                    TempCheckMKT = false;
                }
                else
                {
                    TempCheckMKT = true;
                }

            }
            else
            {
                TempCheckBO = true;
            }




            if (!TempCheckBO && !TempCheckMKT)
            {
                com.Dispose();
                return ChangePasswordStatus.IncorrectUserId;
            }


            //ตรวจสอบ user และ password
            encryptPass = EncryptPassword(pUserId, pPassword); //เข้ารหัสก่อน
            if (!(dt.Rows[0]["UserPass"]).Equals(encryptPass))
            {
                ErrMsg = "Password ไม่ถูกต้อง !!";
                com.Dispose();
                dt.Dispose();
                return ChangePasswordStatus.IncorrectPassword;
            }

            //ตรวจอสอบ NewPassword
            if (string.Compare(newPass1, newPass2, false) != 0)
            {
                ErrMsg = "กรุณาใส่ New Password และ ReEnter New Password ให้เหมือนกัน !!";
                com.Dispose();
                dt.Dispose();
                return ChangePasswordStatus.NewPassNotEqual;
            }

            //ตรวจสอบว่าเหมือนครั้งที่ผ่านมาหรือไม่
            encryptPass = EncryptPassword(pUserId, newPass2); //เข้ารหัสก่อน
            if (!IsNewPassNotSamePrevious(encryptPass))
            {
                ErrMsg = "Password ต้องไม่ซ้ำของเก่า " + GetPolicy("OP") + " ครั้งที่ผ่านมา";
                com.Dispose();
                dt.Dispose();
                return ChangePasswordStatus.SamePreviousPassword;
            }


            com.Dispose();
            dt.Dispose();

            //บันทึกลง Log changepass
            instLogChangePass(encryptPass);

            //update user changepass
            updateUserChangePass(pUserId, encryptPass);

            //update user count changepass
            updateCountUserChangePass(pUserId);

            ErrMsg = "เปลี่ยน Password เรียบร้อย !!!";
            return ChangePasswordStatus.Successed;
        }


        public LogInStatus Login()
        {
            string policyID; int DayExpired; string UserStartDate; int tempUserIncorrectPass; string tempDate; int TimeOfIncorrent; int NonActiveDays; string UserLastLoginDate; string encryptPass;
            //string[] tempUsrProgramCode;
            //bool canUseProgram;
            //canUseProgram = false;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            //string sql = "SELECT UserID,UserName,UserPass,UserInUse,UserLevel,UserStartDate,UserIncorrectPass,CountChgPass,UserLastLogin,UserProgramCode FROM BACKOFFICEUSERCENTER WHERE USERID = @p1";
            string sql = "select * from ( " +
           "SELECT 'SS' as Type,UserID, UserName, UserPass, UserInUse, UserLevel, UserStartDate, UserIncorrectPass, CountChgPass, UserLastLogin, UserProgramCode FROM SsTraderUser " +
           "union " +
           "SELECT 'BO' as Type, UserID, UserName, UserPass, UserInUse, UserLevel, UserStartDate, UserIncorrectPass, CountChgPass, UserLastLogin, UserProgramCode FROM BackOfficeUserCenter " +
           ") A " +
           " where A.UserID = @p1 ";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            com.Parameters.AddWithValue("@p1", pUserId);
            dt = db.GetDataTable(com);
            com.Parameters.Clear();

            //ถ้าไม่มี User ในฐานข้อมูล
            if (dt == null)
            {
                ErrMsg = "User Id ไม่ถูกต้องกรุณาตรวจสอบอีกครั้ง !!";
                com.Dispose();
                return LogInStatus.Failed;
            }

            if (dt.Rows.Count == 0)
            {
                ErrMsg = "User Id ไม่ถูกต้องกรุณาตรวจสอบอีกครั้ง !!";
                com.Dispose();
                return LogInStatus.Failed;
            }

            //เช็คว่าถูกระงับการใช้งานหรือไม่
            if (dt.Rows[0]["UserInUse"].Equals("N"))
            {
                ErrMsg = "User ของคุณถูกระงับการใช้งาน !!";
                com.Dispose();
                dt.Dispose();
                return LogInStatus.Suspended;
            }

            //เช็คว่ามีสิทธิใช้งานโปรแกรมนีหรือไม่ หน้า Portal ไม่ต้องเช็ค
            //tempUsrProgramCode = Convert.ToString(dt.Rows[0]["UserProgramCode"]).Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
            //foreach(string Strtemp in tempUsrProgramCode)
            //{
            //    if(Strtemp.Equals(pProgramCode,StringComparison.OrdinalIgnoreCase))
            //    {
            //        canUseProgram = true;
            //    }
            //}
            //if (canUseProgram == false)
            //{
            //    ErrMsg = "คุณไม่มีสิทธิใช้งานโปรแกรมนี้ !!";
            //    com.Dispose();
            //    dt.Dispose();
            //    return LogInStatus.NotPermissionToUse;
            //}

            //ตรวจสอบ user และ password
            encryptPass = EncryptPassword(pUserId, pPassword); //เข้ารหัสก่อน
            if (!(dt.Rows[0]["UserPass"]).Equals(encryptPass))
            {
                ErrMsg = "Password ไม่ถูกต้องกรุณาลองใหม่อีกครั้ง !!";
                tempUserIncorrectPass = Convert.ToInt32(dt.Rows[0]["UserIncorrectPass"]);
                com.Dispose();
                dt.Dispose();
                UpdateIncorrectPass(pUserId, tempUserIncorrectPass + 1);
                return LogInStatus.Failed;
            }

            //เช็คว่า Password หมดอายุหรือไม่
            //เฉพาะBO
            if (dt.Rows[0]["Type"].ToString() == "BO")
            {
                if (Convert.ToString(dt.Rows[0]["UserLevel"]) == "00")
                {
                    policyID = "EA";
                }
                else
                {
                    policyID = "ED";
                }
                DayExpired = GetPolicy(policyID);
                UserStartDate = Convert.ToString(dt.Rows[0]["UserStartDate"]);
                tempDate = db.GetServerdate().Subtract(new TimeSpan(DayExpired, 0, 0, 0)).ToString("yyyyMMdd", new CultureInfo("en-US"));
                if (string.Compare(tempDate, UserStartDate, StringComparison.Ordinal) > -1)
                {
                    ErrMsg = "User Expired กรุณาเปลี่ยน Password ก่อนเข้าใช้งานระบบ";
                    com.Dispose();
                    dt.Dispose();
                    return LogInStatus.Expired;
                }
                //เช็คว่าใส่พาสเวิดผิดเกินจำนวนครั้งที่กำหนดหรือไม่
                TimeOfIncorrent = GetPolicy("IP");
                if (Convert.ToString(dt.Rows[0]["UserLevel"]) != "00")
                {
                    if (Convert.ToInt32(dt.Rows[0]["UserIncorrectPass"]) >= TimeOfIncorrent)
                    {
                        ErrMsg = "คุณ Login ผิดพลาดเกิน " + TimeOfIncorrent + " !!";
                        com.Dispose();
                        dt.Dispose();
                        return LogInStatus.IncorrectPasswordOver;
                    }
                }
                //โดนบังคับเปลี่ยนพาสเวิร์ดหรือไม่
                if (Convert.ToInt32(dt.Rows[0]["CountChgPass"]) == 0)
                {
                    ErrMsg = "กรุณาเปลี่ยน Password ก่อนเข้าใช้งานระบบ !!";
                    com.Dispose();
                    dt.Dispose();
                    return LogInStatus.ForceChgPassword;
                }

                NonActiveDays = GetPolicy("EU");
                UserLastLoginDate = Convert.ToString(dt.Rows[0]["UserLastLogin"]);
                tempDate = db.GetServerdate().Subtract(new TimeSpan(NonActiveDays, 0, 0, 0)).ToString("yyyyMMdd", new CultureInfo("en-US"));
                //เช็คว่าไม่ได้เข้าใช้งานนานเกินกว่ากำหนดหรือไม่
                if (string.Compare(tempDate, UserLastLoginDate, StringComparison.Ordinal) > -1)
                {
                    ErrMsg = "คุณไม่ได้ใช้งานนานเกินกว่า " + NonActiveDays + " วัน !!";
                    com.Dispose();
                    dt.Dispose();
                    return LogInStatus.NonActive;
                }
                ////เครียพาสเวิร์ดผิดให้เป็น 0
                //UpdateIncorrectPass(pUserId, 0);
                //pUserName = dt.Rows[0]["UserName"].ToString();
                //com.Dispose();
                //dt.Dispose();     
            }
            //เครียพาสเวิร์ดผิดให้เป็น 0
            UpdateIncorrectPass(pUserId, 0);
            pUserName = dt.Rows[0]["UserName"].ToString();
            com.Dispose();
            dt.Dispose();
            return LogInStatus.Successed;
        }

    }
}