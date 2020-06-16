//using ConsentManagement.Services;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;

//using static ConsentManament.Models.ConsentManageModels;

//namespace ConsentManament.Services
//{
//    public class ConsentManageServices: IConsentManageServices
//    {
//        public static string pLoginIp;
//        private string sql;
//        private DBUtility _db;
//        SqlCommand com = new SqlCommand();
//        DataTable dt;
//        public ConsentManageServices(DBUtility db)
//        {
//            _db = db;
//        }
//        //public List<ShowCustomerAcceptedConsent> GetCustomerAcceptConsent()
//        //{
//        //    string sql;
//        //    List<ShowCustomerAcceptedConsent> ic = new List<ShowCustomerAcceptedConsent>();
//        //    sql = "select * from dbo.udf_cnms_web_get_customer_consent_list() ";
//        //    com.CommandType = CommandType.Text;
//        //    com.CommandText = sql;
//        //    dt = _db.GetDataTable(com);

//        //    if (dt.Rows.Count != 0)
//        //    {
//        //        foreach (DataRow dr in dt.Rows)
//        //        {
//        //            var m = new ShowCustomerAcceptedConsent();
//        //            m.transactionDate=Convert.ToDateTime(dr["transaction_date"].ToString());
//        //            m.accountNo = dr["account_no"].ToString();
//        //            m.refNo = dr["ref_no"].ToString();
//        //            m.customerName=dr["product_name"].ToString();
//        //            m.productName = dr["customer_name"].ToString();
//        //            m.consentFormName = dr["consent_form_name"].ToString();
//        //            m.status = dr["status"].ToString();
//        //            ic.Add(m);
//        //        }
//        //    }
//        //    com.Parameters.Clear();
//        //    dt.Dispose();
//        //    com.Dispose();
//        //    return ic;
//        //}
//        //public QueryCustomerAcceptedConsentModel GetCustomerAcceptedConsent(QueryCustomerConsentDetailRequestModel req)
//        //{
//        //    QueryCustomerAcceptedConsentModel m = new QueryCustomerAcceptedConsentModel();
//        //    ValidCustomerAcceptedConsentModel valid = new ValidCustomerAcceptedConsentModel();
//        //    valid = GetValidCustomerAcceptedConsent(req.accountNo);

//        //    if (!valid.valid)
//        //    {
//        //        m.remark = "Invalid customer accepted consent.";
//        //        return m;
//        //    }
//        //    m.consentAcceptedForm = GetCustomerAcceptedConsentForm(valid.accountNo);
//        //    m.consentAcceptedDetail = GetCustomerAcceptedConsentDetail(valid.accountNo).OrderBy(o => o.Consent.consentOrder).ToList();
//        //    m.remark = "";
//        //    return m;
//        //}
//        //public CustomerAcceptedConsentFormModel GetCustomerAcceptedConsentForm(string accountNo)
//        //{
//        //    //string sql;
//        //    var m = new CustomerAcceptedConsentFormModel();
//        //    sql = " select * from dbo.udf_cnms_get_customer_consent_form(@paccount_no)";
//        //    com.CommandType = CommandType.Text;
//        //    com.CommandText = sql;
//        //    com.Parameters.AddWithValue("@paccount_no", accountNo);
//        //    dt = _db.GetDataTable(com);
//        //    if (dt.Rows.Count != 0)
//        //    {
//        //        foreach (DataRow dr in dt.Rows)
//        //        {
//        //            m.consentFormID = Guid.Parse(dr["consent_form_id"].ToString());

//        //            m.accountNo = dr["account_no"].ToString();
//        //            m.transactionID = Guid.Parse(dr["id"].ToString());
//        //            //m.systemName = dr["system_name"].ToString();
//        //            //m.userIpAddress = dr["ip_address"].ToString();
//        //            //m.maxAddress = dr["mac_address"].ToString();
//        //            m.consentDate = Convert.ToDateTime(dr["consent_date"].ToString());
//        //            m.consentFormName = dr["consent_form_name"].ToString();
//        //            m.titleName = dr["consent_title"].ToString();
//        //            m.consentHeader = dr["consent_form_header"].ToString();
//        //            m.consentFooter = dr["consent_form_footer"].ToString();
//        //            //if (dr["is_use"].ToString() == "Y")
//        //            //    m.isUse = true;
//        //            //else
//        //            //    m.isUse = false;
//        //            //m.accountNo = dr["consent_date"].ToString();
//        //        }
//        //    }

//        //    com.Parameters.Clear();
//        //    com.Dispose();
//        //    return m;
//        //}
//        //public List<CustomerAcceptedConsentDetailModel> GetCustomerAcceptedConsentDetail(Guid consentFormId, string accountNo)
//        //{
//        //    string sql;
//        //    List<CustomerAcceptedConsentDetailModel> ls = new List<CustomerAcceptedConsentDetailModel>();
//        //    sql = " select * from dbo.udf_cnms_get_customer_accepted_detail(@pconsent_form_id,@paccount_no) ";
//        //    com.CommandType = CommandType.Text;
//        //    com.CommandText = sql;
//        //    com.Parameters.AddWithValue("@pconsent_form_id", consentFormId);
//        //    com.Parameters.AddWithValue("@paccount_no", accountNo);
//        //    dt = _db.GetDataTable(com);
//        //    if (dt.Rows.Count != 0)
//        //    {
//        //        foreach (DataRow dr in dt.Rows)
//        //        {
//        //            var m = new CustomerAcceptedConsentDetailModel();
//        //            m.Consent = GetConsentById(consentFormId, Guid.Parse(dr["consent_id"].ToString()));
//        //            if (dr["is_accepted"].ToString() == "Y")
//        //                m.IsAccept = true;
//        //            else
//        //                m.IsAccept = false;
//        //            //m.AccountNo = dr["account_no"].ToString();
//        //            //m.DateTime = Convert.ToDateTime(dr["create_datetime"].ToString());
//        //            ls.Add(m);
//        //        }
//        //    }
//        //    com.Parameters.Clear();
//        //    dt.Dispose();
//        //    com.Dispose();
//        //    return ls;
//        //}
//        //    public ValidCustomerAcceptedConsentModel GetValidCustomerAcceptedConsent(string accountNo)
//        //    {
//        //        string sql;
//        //        var m = new ValidCustomerAcceptedConsentModel();
//        //        sql = " select * from dbo.udf_cnms_get_valid_customer_accepted_consent(@paccount_no) ";
//        //        com.Parameters.AddWithValue("@paccount_no", accountNo);
//        //        com.CommandType = CommandType.Text;
//        //        com.CommandText = sql;
//        //        dt = _db.GetDataTable(com);
//        //        if (dt.Rows.Count != 0)
//        //        {
//        //            foreach (DataRow dr in dt.Rows)
//        //            {
//        //                m.consentFormID = Guid.Parse(dt.Rows[0]["consent_form_id"].ToString());
//        //                m.accountNo = dt.Rows[0]["account_no"].ToString();
//        //                m.valid = true;
//        //            }
//        //        }
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        return m;
//        //    }
//        //    public ConsentModel GetConsentById(Guid consentFormId, Guid consentID)
//        //    {
//        //        string sql;
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        var m = new ConsentModel();
//        //        sql = " select * from dbo.udf_cnms_get_consent_list(@pconsent_form_id) where consent_id = @pconsent_id ";
//        //        com.Parameters.AddWithValue("@pconsent_form_id", consentFormId);
//        //        com.Parameters.AddWithValue("@pconsent_id", consentID);
//        //        com.CommandType = CommandType.Text;
//        //        com.CommandText = sql;
//        //        dt = _db.GetDataTable(com);
//        //        if (dt.Rows.Count != 0)
//        //        {
//        //            foreach (DataRow dr in dt.Rows)
//        //            {
//        //                m.consentID = Guid.Parse(dr["consent_id"].ToString());
//        //                //m.ConsentFormID = Guid.Parse(dr["consent_form_id"].ToString());
//        //                m.titleName = dr["title_name"].ToString();
//        //                m.consentOrder = Convert.ToInt32(dr["consent_order"].ToString());
//        //                m.contentHtml = dr["content_html"].ToString();
//        //                m.contentText = dr["content_text"].ToString();
//        //                m.remark = dr["remark"].ToString();
//        //                m.createDate = Convert.ToDateTime(dr["create_date"].ToString());
//        //            }
//        //        }
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        return m;
//        //    }

//        //    public ShowDashBoard1 GetDashBoard1 ()
//        //    {
//        //        ShowDashBoard1 m = new ShowDashBoard1();
//        //        string sql;
//        //        com.Parameters.Clear();
//        //        com.Dispose();
//        //        sql = " select * from dbo.udf_cnms_web_get_dash_board_1() ";
//        //        com.CommandType = CommandType.Text;
//        //        com.CommandText = sql;
//        //        dt = _db.GetDataTable(com);
//        //        if (dt.Rows.Count != 0)
//        //        {
//        //            foreach (DataRow dr in dt.Rows)
//        //            {
//        //                m.total = Convert.ToDecimal(dr["total"].ToString());
//        //                m.fullAccepted = Convert.ToDecimal(dr["full_accepted"].ToString());
//        //                m.partialAccepted = Convert.ToDecimal(dr["partial_accepted"].ToString());
//        //                m.notAccepted = Convert.ToDecimal(dr["not_accepted"].ToString());
//        //            }
//        //        }
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        return m;
//        //    }

//        //    public ShowDashBoard2 GetDashBoard2()
//        //    {
//        //        ShowDashBoard2 m = new ShowDashBoard2();
//        //        m.consentForm = new List<ConsentFormModel>();
//        //        string sql;
//        //        com.Parameters.Clear();
//        //        com.Dispose();
//        //        sql = " select * from dbo.udf_cnms_get_web_consent_form_list() ";
//        //        com.CommandType = CommandType.Text;
//        //        com.CommandText = sql;
//        //        dt = _db.GetDataTable(com);
//        //        if (dt.Rows.Count != 0)
//        //        {
//        //            foreach (DataRow dr in dt.Rows)
//        //            {
//        //                var mm = new ConsentFormModel();
//        //                mm.consentFormName = dr["consent_form_name"].ToString();
//        //                mm.titleName = dr["consent_title"].ToString();
//        //                mm.consentHeader = dr["consent_form_header"].ToString();
//        //                mm.consentFooter = dr["consent_form_footer"].ToString();
//        //                mm.consentFormID = Guid.Parse(dr["consent_form_id"].ToString());
//        //                mm.consent = GetConsentDetail(mm.consentFormID);
//        //                m.consentForm.Add(mm);
//        //            }
//        //        }
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        return m;
//        //    }
//        //    private List<ConsentModel> GetConsentDetail(Guid consentFormId)
//        //    {
//        //        List<ConsentModel> data = new List<ConsentModel>();
//        //        sql = "select * from dbo.udf_cnms_get_consent_list(@pconsent_form_id) ";
//        //        //com.Parameters.AddWithValue("@pconsent_form_id", req.consentFormId);
//        //        com.Parameters.AddWithValue("@pconsent_form_id", consentFormId);
//        //        com.CommandType = CommandType.Text;
//        //        com.CommandText = sql;

//        //        dt = _db.GetDataTable(com);
//        //        if (dt.Rows.Count != 0)
//        //        {
//        //            foreach (DataRow dr in dt.Rows)
//        //            {
//        //                var m = new ConsentModel();
//        //                var mm = new ConsentStatusModel();
//        //                m.consentID = Guid.Parse(dr["consent_id"].ToString());
//        //                //m.ConsentFormID = Guid.Parse(dr["consent_form_id"].ToString());
//        //                m.titleName = dr["title_name"].ToString();
//        //                m.consentOrder = Convert.ToInt32(dr["consent_order"].ToString());
//        //                m.contentHtml = dr["content_html"].ToString();
//        //                m.contentText = dr["content_text"].ToString();
//        //                m.remark = dr["remark"].ToString();
//        //                m.createDate = Convert.ToDateTime(dr["create_date"].ToString());
//        //                mm = GetConsentPercent(m.consentID);
//        //                m.consentStatus = mm;
//        //                //m.percent = GetConsentPercent(m.consentID);
//        //                data.Add(m);
//        //            }
//        //        }
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        return data.OrderBy(o => o.consentOrder).ToList(); ;
//        //    }
//        //    public ConsentStatusModel GetConsentPercent( Guid consentID)
//        //    {
//        //        ConsentStatusModel m = new ConsentStatusModel();
//        //        string sql;
//        //        com.Parameters.Clear();
//        //        com.Dispose();
//        //        sql = " select * from dbo.udf_cnms_web_get_consent_percent(@pconsent_id) ";
//        //        com.Parameters.AddWithValue("@pconsent_id", consentID);
//        //        com.CommandType = CommandType.Text;
//        //        com.CommandText = sql;
//        //        dt = _db.GetDataTable(com);
//        //        if (dt.Rows.Count != 0)
//        //        {
//        //            foreach (DataRow dr in dt.Rows)
//        //            {
//        //                m.total= Convert.ToDecimal(dr["total"].ToString());
//        //                m.accepted = Convert.ToDecimal(dr["accepted"].ToString());
//        //                m.notAccepted = Convert.ToDecimal(dr["notAccepted"].ToString());
//        //                m.percent = Convert.ToDecimal(dr["ppercent"].ToString());
//        //            }
//        //        }
//        //        com.Parameters.Clear();
//        //        dt.Dispose();
//        //        com.Dispose();
//        //        return m;
//        //    }

//        //    public List<ConsentFormModel> GetConsentForm()
//        //    {
//        //        List<ConsentFormModel> ls = new List<ConsentFormModel>();
//        //        //HttpClient.DefaultRequestHeaders.Authorization =
//        //        //         new AuthenticationHeaderValue("Bearer", "Your Oauth token");
//        //        return ls;
//        //    }

//    }
//    public interface IConsentManageServices
//        {
//            List<ShowCustomerAcceptedConsent> GetCustomerAcceptConsent();

//            List<ConsentFormModel> GetConsentForm();
//            QueryCustomerAcceptedConsentModel GetCustomerAcceptedConsent(QueryCustomerConsentDetailRequestModel req);
//            ShowDashBoard1 GetDashBoard1();
//            ShowDashBoard2 GetDashBoard2();
//        }

//    }
