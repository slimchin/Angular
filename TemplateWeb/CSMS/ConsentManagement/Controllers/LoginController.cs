using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsentManagement.Models;
using System.Web.Security;
using ConsentManagement;
using System.Web.Configuration;
using System.Text.RegularExpressions;
//using ConsentManagement.Services;
using ConsentManagement.Models.UsersManagement;
using Novell.Directory.Ldap;
using System.Data.SqlClient;
using System.Data;
using ConsentManament.Models;

namespace ConsentManagement.Controllers
{
   
    public class LoginController : Controller
    {
        public static string pLoginIp;
        public static string ProgramCode = "ConsentManagement";
        public static string ProgramVersion = "1.01";
        private static string pIp = "";
        private readonly IAuthenticationService _authen;
        //private readonly IActivityServices _activity;
        ActivityModel act = new ActivityModel();

        //public LoginController()
        //{
        //    _authen = authen;
        //    _activity = activity;
        //}

        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = ProgramCode + "Login";
            return View("Index");
        }

        public ActionResult Logout()
        {
            act.Username = Session["usr"].ToString();
            act.Activity = "Logout";
            act.Remark = "Success";
            //_activity.saveActivityLog(act);

            Session.Abandon();
            //return View("Index");
            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult LogInProcess(FormCollection col)
        {
            string tempConstr = WebConfigurationManager.ConnectionStrings["DBSession"].ToString();
            string tempIp = Regex.Match(tempConstr, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b").Value;

            pIp = tempIp;

            LoginStatusModel login = new LoginStatusModel();
            
            string username;/*= "backofficeservice@asiaplus.co.th";*/
            username = col["user"];
            string s2 = "assetfund.co.th";
            bool b = username.Contains(s2);
            if (!b)
            {
                username = username.Replace("@asiaplus.co.th", "");
                username = username + "@asiaplus.co.th";
            }
            string password;/* = "dP3fdjk#4d8*dR";*/
            password  = col["password"];
            if (LdapAuthen(username, password))
            {
                if(UserPermission(username))
                {
                    login.LoginDetail = "";
                    login.LoginStatus = 1;

                    //if (_authen.UserPermission(username))
                    Session["UserName"] = username;
                    Session["usr"] = username;
                    Session["ServerIp"] = pIp;
                    act.Username = username;
                    act.Activity = "Login";
                    act.Remark = "Success";
                    //_activity.saveActivityLog(act);
                    
                }else
                {
                    login.LoginDetail = "ไม่สามารถเข้าสู่ระบบได้ เนื่องจากไม่มีสิทธิใช้งานระบบ";
                    login.LoginStatus = 0;
                    Session["UserName"] = null;
                    Session["usr"] = null;
                    Session["ServerIp"] = null;
                    act.Username = username;
                    act.Activity = "Login";
                    act.Remark = "Fail";
                    //_activity.saveActivityLog(act);
                    Session.Abandon();
                }
                
            }
            else
            {
                login.LoginDetail = "ไม่สามารถเข้าสู่ระบบได้ กรุณาตรวจสอบ user และ password ให้ถูกต้อง หรือติดต่อฝ่าย IT";
                login.LoginStatus = 0;
                act.Username = username;
                act.Activity = "Login";
                act.Remark = "Fail";
                //_activity.saveActivityLog(act);
                Session.Abandon();
            }
            return Json(login);
        }

        [HttpPost]
        public RedirectToRouteResult ChangePasswordProcess(string user = "", string pwd = "", string newPwd1 = "", string newPwd2 = "")
        {
            UserManagement u = new UserManagement(pIp, user, pwd, ProgramCode);

            ChangePasswordStatus Result = u.ChangePassword(newPass1: newPwd1, newPass2: newPwd2);

            if (Result == ChangePasswordStatus.IncorrectUserId)
            {
                return RedirectToAction("GotoLoginErrorPage", new { ErrorMsg = u.ErrMsg });
            }
            else if (Result == ChangePasswordStatus.IncorrectPassword)
            {
                return RedirectToAction("GotoLoginErrorPage", new { ErrorMsg = u.ErrMsg });
            }
            else if (Result == ChangePasswordStatus.NewPassNotEqual)
            {
                return RedirectToAction("GotoLoginErrorPage", new { ErrorMsg = u.ErrMsg });
            }
            else if (Result == ChangePasswordStatus.SamePreviousPassword)
            {
                return RedirectToAction("GotoLoginErrorPage", new { ErrorMsg = u.ErrMsg });
            }
            else if (Result == ChangePasswordStatus.Successed)
            {

                //return RedirectToAction("Index");
                return RedirectToAction("GotoSuccessedPage", new { ErrorMsg = u.ErrMsg });
            } 

            return RedirectToAction("GoToHttpNotFound");
        }
        public ActionResult GotoLoginErrorPage(string ErrorMsg)
        {
            ViewBag.Title = "Login Error Page";
            ViewBag.ErrorMsg = ErrorMsg;
            return View("LoginErrorPage");
        }
        public bool LdapAuthen(string loginDn, string password)
        {
            Boolean rs = false;
            string ldapHost = "172.20.50.254";
            int ldapPort = 389;
            try
            {
                LdapConnection ldapConn = new LdapConnection();
                ldapConn.Connect(ldapHost, ldapPort);
                ldapConn.Bind(loginDn, password);
                rs = true;
                ldapConn.Disconnect();
            }
            catch
            {
                rs = false;
            }
            return rs;
        }
        public bool UserPermission(string userLogin)
        {
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;

            com.CommandText = " select * from dbo.udf_cnms_get_user_detail(@p1) ";
            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@p1", userLogin);
            dt = db.GetDataTable(com);
            if (dt.Rows.Count != 0)
                return true;
            else
                return false;

        }
        public ActionResult GotoSuccessedPage(string ErrorMsg)
        {
            ViewBag.Title = "Successed Page";
            ViewBag.ErrorMsg = ErrorMsg;
            return View("SuccessedPage");
        }
        public ActionResult GotoAuthorizeFailPage(string ErrorMsg)
        {
            ViewBag.Title = "Authorize Fail Page";
            ViewBag.ErrorMsg = ErrorMsg;
            return View("AuthorizeFailPage");
        }

        [ActionName("ChangePassword")]
        public ActionResult GotoChangePassword(string MsgShow,string usr)
        {
            ViewBag.Title = "Change Password";
            ViewBag.Msg = MsgShow;
            ViewBag.Usr = usr;
            return View("ChangePassword");
        }
        public ActionResult GoToHttpNotFound()
        {
            return HttpNotFound();
        }

        //[AspAuthen]
        public JsonResult GetMenuPermission()
        {
            //ReadMenuPermission
            UserManagement.pProgramCode = LoginController.ProgramCode;
            List<string> menu = UserManagement.ReadMenuPermission(Session["usr"].ToString());
            return Json(menu, "text/html", JsonRequestBehavior.AllowGet);
            //return Json(new { Sesison = Session["usr"]},"text/html",JsonRequestBehavior.AllowGet);
        }
    }
}