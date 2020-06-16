using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ConsentManament.Models.ConsentManageModels;

namespace ConsentManagement.Controllers
{
    [AspAuthen]
    [AspAuthorize(functionName = "DashBoard")]
    public class HomeController : Controller
    {


        public static string pLoginIp;

        public HomeController()
        {
            //this._ConsentManageServices = ConsentManageServices;
        }

        public ShowDashBoard1 GetDashBoard1()
        {
            string sql;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            ShowDashBoard1 m = new ShowDashBoard1();
            com.Parameters.Clear();
            sql = " select * from dbo.udf_cnms_web_get_dash_board_1() ";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            dt = db.GetDataTable(com);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    m.total = Convert.ToDecimal(dr["total"].ToString());
                    m.fullAccepted = Convert.ToDecimal(dr["full_accepted"].ToString());
                    m.partialAccepted = Convert.ToDecimal(dr["partial_accepted"].ToString());
                    m.notAccepted = Convert.ToDecimal(dr["not_accepted"].ToString());
                }
            }
            com.Parameters.Clear();
            dt.Dispose();
            com.Dispose();
            return m;
        }

        public ShowDashBoard2 GetDashBoard2()
        {
            string sql;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            ShowDashBoard2 m = new ShowDashBoard2();
            m.consentForm = new List<ConsentFormModel>();
            com.Parameters.Clear();
            com.Dispose();
            sql = " select * from dbo.udf_cnms_get_web_consent_form_list() ";
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            dt = db.GetDataTable(com);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var mm = new ConsentFormModel();
                    mm.consentFormName = dr["consent_form_name"].ToString();
                    mm.titleName = dr["consent_title"].ToString();
                    mm.consentHeader = dr["consent_form_header"].ToString();
                    mm.consentFooter = dr["consent_form_footer"].ToString();
                    mm.consentFormID = Guid.Parse(dr["consent_form_id"].ToString());
                    mm.consent = GetConsentDetail(mm.consentFormID);
                    m.consentForm.Add(mm);
                }
            }
            com.Parameters.Clear();
            dt.Dispose();
            com.Dispose();
            return m;
        }
        private List<ConsentModel> GetConsentDetail(Guid consent_form_id)
        {
            string sql;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            List<ConsentModel> data = new List<ConsentModel>();
            sql = "select * from dbo.udf_cnms_get_consent_list_by_form(@pconsent_form_id) ";
            //com.Parameters.AddWithValue("@pconsent_form_id", req.consentFormId);
            com.Parameters.AddWithValue("@pconsent_form_id", consent_form_id);
            com.CommandType = CommandType.Text;
            com.CommandText = sql;

            dt = db.GetDataTable(com);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var m = new ConsentModel();
                    var mm = new ConsentStatusModel();
                    m.consentID = Guid.Parse(dr["consent_id"].ToString());
                    //m.ConsentFormID = Guid.Parse(dr["consent_form_id"].ToString());
                    m.titleName = dr["title_name"].ToString();
                    m.consentOrder = Convert.ToInt32(dr["consent_order"].ToString());
                    m.contentHtml = dr["content_html"].ToString();
                    m.contentText = dr["content_text"].ToString();
                    m.remark = dr["remark"].ToString();
                    m.createDate = Convert.ToDateTime(dr["create_date"].ToString());
                    mm = GetConsentPercent(m.consentID);
                    m.consentStatus = mm;
                    //m.percent = GetConsentPercent(m.consentID);
                    data.Add(m);
                }
            }
            com.Parameters.Clear();
            dt.Dispose();
            com.Dispose();
            return data.OrderBy(o => o.consentOrder).ToList(); ;
        }
        public ConsentStatusModel GetConsentPercent(Guid consentID)
        {
            string sql;
            DBUtility db = new DBUtility(pLoginIp, "Backoffice", 60);
            SqlCommand com = new SqlCommand();
            DataTable dt;
            ConsentStatusModel m = new ConsentStatusModel();
            com.Parameters.Clear();
            com.Dispose();
            sql = " select * from dbo.udf_cnms_web_get_consent_percent(@pconsent_id) ";
            com.Parameters.AddWithValue("@pconsent_id", consentID);
            com.CommandType = CommandType.Text;
            com.CommandText = sql;
            dt = db.GetDataTable(com);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    m.total = Convert.ToDecimal(dr["total"].ToString());
                    m.accepted = Convert.ToDecimal(dr["accepted"].ToString());
                    m.notAccepted = Convert.ToDecimal(dr["notAccepted"].ToString());
                    m.percent = Convert.ToDecimal(dr["ppercent"].ToString());
                }
            }
            com.Parameters.Clear();
            dt.Dispose();
            com.Dispose();
            return m;
        }

        public List<ConsentFormModel> GetConsentForm()
        {
            List<ConsentFormModel> ls = new List<ConsentFormModel>();
            //HttpClient.DefaultRequestHeaders.Authorization =
            //         new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            return ls;
        }
        public ActionResult Dashboard()
        {
            ViewBag.CustomerList = GetDashBoard1();
            ViewBag.DashBoard2 = GetDashBoard2();
            ViewBag.Active = "DashBoard";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}