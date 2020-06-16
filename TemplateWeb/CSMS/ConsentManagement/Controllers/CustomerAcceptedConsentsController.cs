using ConsentManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using static ConsentManament.Models.ConsentManageModels;

namespace ConsentManament.Controllers
{
    [AspAuthen]
    [AspAuthorize(functionName = "CustomerAcceptedConsents")]

    public class CustomerAcceptedConsentsController : Controller
    {
        //private IConsentManageServices _ConsentManageServices;
        string baseUrl = ConfigurationManager.AppSettings["Baseurl"].ToString();
        string systemName = ConfigurationManager.AppSettings["SystemName"].ToString();
        string systemIpAddress = ConfigurationManager.AppSettings["SystemIpAddress"].ToString();
        // GET: ConsentManagement
        public CustomerAcceptedConsentsController()
        {

        }

        #region Production


        public async Task<ActionResult> AcceptedDetailPartial(FormCollection c)
        {
            QueryCustomerAcceptedConsentModel m = new QueryCustomerAcceptedConsentModel();
            //QueryCustomerConsentDetailRequestModel rm = new QueryCustomerConsentDetailRequestModel();
            //rm.accountNo = c["accountNo"];

            using (var client = new HttpClient())
            {
                JObject JsonData = new JObject(
                        new JProperty("accountNo", c["accountNo"]),
                        new JProperty("systemName", systemName),
                        new JProperty("systemIpAddress", systemIpAddress)
                    );

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(JsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.GetAsync(baseUrl + "/customers/" + c["accountNo"]);

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    m = JsonConvert.DeserializeObject<QueryCustomerAcceptedConsentModel>(Response);
                }
                return PartialView("AcceptedDetailPartial", m);
            }
        }
        public async Task<ActionResult> List()
        {
            List<ShowCustomerAcceptedConsent> ls = new List<ShowCustomerAcceptedConsent>();
            List<ConsentFormHeaderModel> model = new List<ConsentFormHeaderModel>();
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));



            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddDays(-30);
            string startdateSel = startDate.ToString("dd/MM/yyyy");
            string endDateSel = endDate.ToString("dd/MM/yyyy");

            using (var client = new HttpClient())
            {

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent("", Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.GetAsync(baseUrl + "/Customers");

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    ls = JsonConvert.DeserializeObject<List<ShowCustomerAcceptedConsent>>(Response);
                }
                var httpResponse2 = await client.GetAsync(baseUrl + "/forms");
                if (httpResponse2.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response2 = httpResponse2.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    model = JsonConvert.DeserializeObject<List<ConsentFormHeaderModel>>(Response2);
                }
                List<SelectListItem> consentFormItems = new List<SelectListItem>();

                consentFormItems.Add(new SelectListItem { Text = "---All---", Value = "" });
                model.ForEach(delegate (ConsentFormHeaderModel m)
                {
                    consentFormItems.Add(new SelectListItem { Text = m.consentFormName, Value = m.consentFormName });
                });
                ViewBag.accountNo = "";
                ViewBag.searchCondition = "Last 30 Days";
                ViewBag.startdateSel = startdateSel;
                ViewBag.endDateSel = endDateSel;
                ViewBag.CustomerList = ls;
                ViewBag.ConsentForm = consentFormItems;
                ViewBag.Active = "CustomerAcceptedList";
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Search(FormCollection c)
        {
            List<ShowCustomerAcceptedConsent> ls = new List<ShowCustomerAcceptedConsent>();
            List<ConsentFormHeaderModel> model = new List<ConsentFormHeaderModel>();
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            string searchCondition = "";

            SearchCustomerAcceptedConsentRequestModel data = new SearchCustomerAcceptedConsentRequestModel();
            data.accountNo = c["accountNo"];
            if (c["accountNo"] != "")
            {
                searchCondition = searchCondition + "AccountNo.:" + c["accountNo"] + "  ";
                ViewBag.accountNo = c["accountNo"];
            }

            data.consentFormName = c["consentFormName"];
            if (c["consentFormName"] != "")
                searchCondition = searchCondition + " Consent Form Name:" + c["consentFormName"] + "  ";
            data.productName = "";
            data.startDate = DateTimeConvertUtility.CnvDateShowToDb(c["TXTStartDate"]);
            data.endDate = DateTimeConvertUtility.CnvDateShowToDb(c["TXTEndDate"]);
            searchCondition = searchCondition + " Transaction date between " + c["TXTStartDate"] + " to " + c["TXTEndDate"];
            var jsonData = JsonConvert.SerializeObject(data);


            string startdateSel = c["TXTStartDate"];
            string endDateSel = c["TXTEndDate"];

            using (var client = new HttpClient())
            {

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.PostAsync(baseUrl + "/customers/query", httpContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    ls = JsonConvert.DeserializeObject<List<ShowCustomerAcceptedConsent>>(Response);
                }
                var httpResponse2 = await client.GetAsync(baseUrl + "/forms");
                if (httpResponse2.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response2 = httpResponse2.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    model = JsonConvert.DeserializeObject<List<ConsentFormHeaderModel>>(Response2);
                }
                List<SelectListItem> consentFormItems = new List<SelectListItem>();
                if (c["consentFormName"] == "")
                    consentFormItems.Add(new SelectListItem { Text = "---All---", Value = c["consentFormName"] });
                else
                {
                    consentFormItems.Add(new SelectListItem { Text = c["consentFormName"], Value = c["consentFormName"] });
                    consentFormItems.Add(new SelectListItem { Text = "---All---", Value = c["consentFormName"] });
                }

                model.ForEach(delegate (ConsentFormHeaderModel m)
                {
                    if (c["consentFormName"] != m.consentFormName)
                        consentFormItems.Add(new SelectListItem { Text = m.consentFormName, Value = m.consentFormName });

                });



                ViewBag.searchCondition = searchCondition;
                ViewBag.startdateSel = startdateSel;
                ViewBag.endDateSel = endDateSel;
                ViewBag.CustomerList = ls;
                ViewBag.ConsentForm = consentFormItems;
                ViewBag.Active = "CustomerAcceptedList";
                return View("List");
            }
        }
        public async Task<ActionResult> Export(FormCollection c)
        {
            List<QueryCustomerAcceptedConsentModel> ls = new List<QueryCustomerAcceptedConsentModel>();
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            string searchCondition = "";

            SearchCustomerAcceptedConsentRequestModel data = new SearchCustomerAcceptedConsentRequestModel();
            data.accountNo = c["accountNo"];
            if (c["accountNo"] != "")
            {
                searchCondition = searchCondition + "AccountNo.:" + c["accountNo"] + "  ";
                ViewBag.accountNo = c["accountNo"];
            }

            data.consentFormName = c["consentFormName"];
            if (c["consentFormName"] != "")
                searchCondition = searchCondition + " Consent Form Name:" + c["consentFormName"] + "  ";
            data.productName = "";
            data.startDate = DateTimeConvertUtility.CnvDateShowToDb(c["TXTStartDate"]);
            data.endDate = DateTimeConvertUtility.CnvDateShowToDb(c["TXTEndDate"]);
            searchCondition = searchCondition + " Transaction date between " + c["TXTStartDate"] + " to " + c["TXTEndDate"];
            var jsonData = JsonConvert.SerializeObject(data);


            string startdateSel = c["TXTStartDate"];
            string endDateSel = c["TXTEndDate"];

            using (var client = new HttpClient())
            {

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json");



                var httpResponse = await client.PostAsync(baseUrl + "/customers/export", httpContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response1 = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    ls = JsonConvert.DeserializeObject<List<QueryCustomerAcceptedConsentModel>>(Response1);
                }


                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Export");
                int i;
                i = 2;
                workSheet.Cells["A1"].Value = "AccountNo";
                workSheet.Cells["B1"].Value = "Product";
                workSheet.Cells["C1"].Value = "Consent1";
                workSheet.Cells["D1"].Value = "Consent2";
                workSheet.Cells["E1"].Value = "Consent3";
                workSheet.Cells["F1"].Value = "Consent4";
                foreach (QueryCustomerAcceptedConsentModel m1 in ls)
                {
                    workSheet.Cells["A" + i].Value = m1.consentAcceptedForm.accountNo;
                    workSheet.Cells["B" + i].Value = m1.consentAcceptedForm.productName;
                    foreach (CustomerAcceptedConsentDetailModel m2 in m1.consentAcceptedDetail)
                    {
                        if (m2.Consent.consentOrder == 1)
                            if (m2.IsAccept)
                                workSheet.Cells["C" + i].Value = "Accepted";
                            else
                                workSheet.Cells["C" + i].Value = "Not Accepted";

                        if (m2.Consent.consentOrder == 2)
                            if (m2.IsAccept)
                                workSheet.Cells["D" + i].Value = "Accepted";
                            else
                                workSheet.Cells["D" + i].Value = "Not Accepted";
                        if (m2.Consent.consentOrder == 3)
                            if (m2.IsAccept)
                                workSheet.Cells["E" + i].Value = "Accepted";
                            else
                                workSheet.Cells["E" + i].Value = "Not Accepted";
                        if (m2.Consent.consentOrder == 4)
                            if (m2.IsAccept)
                                workSheet.Cells["F" + i].Value = "Accepted";
                            else
                                workSheet.Cells["F" + i].Value = "Not Accepted";
                    }
                    i++;
                }
                --i;
                workSheet.Cells["A1:F" + i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["A1:F" + i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["A1:F" + i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells["A1:F" + i].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            

                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Customers accepted consent.xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return Json(new { IsSuccessed = true, RespondMessage = "Export Data Completed!!", RespondData = "" }, "application/json", JsonRequestBehavior.AllowGet);

            }
        }
        public async Task<ActionResult> Create()
        {
            List<ConsentFormHeaderModel> model = new List<ConsentFormHeaderModel>();

            using (var client = new HttpClient())
            {

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent("", Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.GetAsync(baseUrl + "/forms");

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    model = JsonConvert.DeserializeObject<List<ConsentFormHeaderModel>>(Response);
                }
                List<SelectListItem> consentFormItems = new List<SelectListItem>();
                model.ForEach(delegate (ConsentFormHeaderModel m)
                {
                    consentFormItems.Add(new SelectListItem { Text = m.consentFormName, Value = m.consentFormID.ToString() });
                });


                ViewBag.ConsentFormList = model;
                ViewBag.ConsentForm = consentFormItems;
                return View(model);
            }
        }
        public async Task<ActionResult> Edit(FormCollection c)
        {
            QueryCustomerAcceptedConsentModel m = new QueryCustomerAcceptedConsentModel();
            string consentIDStr = "";
            string accountNo = c["ACC"];
            using (var client = new HttpClient())
            {
                JObject JsonData = new JObject(
                        new JProperty("accountNo", accountNo),
                        new JProperty("systemName", systemName),
                        new JProperty("systemIpAddress", systemIpAddress)
                    );

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(JsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.GetAsync(baseUrl + "/customers/" + accountNo);

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    m = JsonConvert.DeserializeObject<QueryCustomerAcceptedConsentModel>(Response);
                }
                foreach (CustomerAcceptedConsentDetailModel mm in m.consentAcceptedDetail)
                {
                    consentIDStr = consentIDStr + mm.Consent.consentID.ToString() + ",";
                }
                ViewBag.ConsentIDStr = consentIDStr;

                return View(m);
            }
        }

        public async Task<ActionResult> Delete(FormCollection c)
        {
            QueryCustomerAcceptedConsentModel m = new QueryCustomerAcceptedConsentModel();
            string consentIDStr = "";
            string accountNo = c["ACC"];
            using (var client = new HttpClient())
            {
                JObject JsonData = new JObject(
                        new JProperty("accountNo", accountNo),
                        new JProperty("systemName", systemName),
                        new JProperty("systemIpAddress", systemIpAddress)
                    );

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(JsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.GetAsync(baseUrl + "/customers/" + accountNo);

                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    m = JsonConvert.DeserializeObject<QueryCustomerAcceptedConsentModel>(Response);
                }
                foreach (CustomerAcceptedConsentDetailModel mm in m.consentAcceptedDetail)
                {
                    consentIDStr = consentIDStr + mm.Consent.consentID.ToString() + ",";
                }
                ViewBag.ConsentIDStr = consentIDStr;

                return View(m);
            }
        }
        public async Task<ActionResult> ConsentFormSelectPartial(FormCollection c)
        {
            ConsentFormModel model = new ConsentFormModel();

            string consentFormIdStr;
            string consentIDStr = "";
            using (var client = new HttpClient())
            {
                if (c["consentFormID"] == null)
                    consentFormIdStr = "6C074227-BE2C-4592-8775-26932ACA5DDE";
                else
                    consentFormIdStr = c["consentFormID"];
                JObject JsonData = new JObject(
                        new JProperty("consentFormId", consentFormIdStr),
                        new JProperty("systemName", systemName),
                        new JProperty("systemIpAddress", systemIpAddress)
                    );

                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(JsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.GetAsync(baseUrl + "/forms/" + consentFormIdStr);


                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    try
                    {
                        model = JsonConvert.DeserializeObject<ConsentFormModel>(Response);
                    }
                    catch (Exception ex)
                    {

                    }

                }

                foreach (ConsentModel mm in model.consent)
                {
                    consentIDStr = consentIDStr + mm.consentID.ToString() + ",";
                }
                ViewBag.ConsentDetail = model;
                ViewBag.ConsentIDStr = consentIDStr;
                return PartialView("ConsentFormSelectPartial");
            }
        }
        public async Task<ActionResult> ConsentSaveResultPartial(FormCollection c)
        {
            ConsentFormModel model = new ConsentFormModel();
            //ConsentFormModel model2 = new ConsentFormModel();
            //model2 = await GetconsentID(Guid.Parse(c["consentFormID"]));
            UpdateCustomerAcceptedConsentRequestModel data = new UpdateCustomerAcceptedConsentRequestModel();
            data.accountNo = c["accountNo"];
            data.consentFormId = Guid.Parse(c["consentFormID"]);
            IList<string> consentSel = c["consentID"].Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            IList<string> consentAll = c["consentIDStr"].Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            List<SaveCustomerAcceptedConsentDetailModel> ls = new List<SaveCustomerAcceptedConsentDetailModel>();

            //foreach (ConsentModel x in model2.consent)
            //{
            try
            {
                foreach (string ord in consentAll)
                {

                    SaveCustomerAcceptedConsentDetailModel m = new SaveCustomerAcceptedConsentDetailModel();
                    m.consentId = Guid.Parse(ord);
                    var match = consentSel.FirstOrDefault(stringToCheck => stringToCheck.Contains(ord));

                    if (match != null)
                        m.isAccepted = true;
                    else
                        m.isAccepted = false;
                    ls.Add(m);
                }
            }
            catch (Exception ee)
            {

            }
            data.consentAccepted = ls;
            data.consentDate = DateTime.Now;
            data.systemName = "CNMS";
            data.systemIpAddress = "0.0.0.0";
            data.customerIpAddress = "0.0.0.0";
            data.remark = "Test";
            data.productName = "Equity";

            var jsonData = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.PostAsync(baseUrl + "/customers/", httpContent);


                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    try
                    {
                        model = JsonConvert.DeserializeObject<ConsentFormModel>(Response);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                ViewBag.ConsentSaveResultPartial = model;
                return PartialView("ConsentSaveResultPartial");
            }
        }
        public async Task<ActionResult> ConsentUpdateResultPartial(FormCollection c)
        {
            ConsentFormModel model = new ConsentFormModel();
            //ConsentFormModel model2 = new ConsentFormModel();
            //model2 = await GetconsentID(Guid.Parse(c["consentFormID"]));
            UpdateCustomerAcceptedConsentRequestModel data = new UpdateCustomerAcceptedConsentRequestModel();
            data.accountNo = c["accountNo"];
            data.consentFormId = Guid.Parse(c["consentFormID"]);
            IList<string> consentSel = c["consentID"].Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            IList<string> consentAll = c["consentIDStr"].Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            List<SaveCustomerAcceptedConsentDetailModel> ls = new List<SaveCustomerAcceptedConsentDetailModel>();

            //foreach (ConsentModel x in model2.consent)
            //{
            try
            {
                foreach (string ord in consentAll)
                {

                    SaveCustomerAcceptedConsentDetailModel m = new SaveCustomerAcceptedConsentDetailModel();
                    m.consentId = Guid.Parse(ord);
                    var match = consentSel.FirstOrDefault(stringToCheck => stringToCheck.Contains(ord));

                    if (match != null)
                        m.isAccepted = true;
                    else
                        m.isAccepted = false;
                    ls.Add(m);
                }
            }
            catch (Exception ee)
            {

            }
            data.consentAccepted = ls;
            data.consentDate = DateTime.Now;
            data.systemName = "CNMS";
            data.systemIpAddress = "0.0.0.0";
            data.customerIpAddress = "0.0.0.0";
            data.remark = "Test";
            data.productName = "Equity";

            var jsonData = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.PutAsync(baseUrl + "/customers/", httpContent);


                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    try
                    {
                        model = JsonConvert.DeserializeObject<ConsentFormModel>(Response);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                ViewBag.ConsentSaveResultPartial = model;
                return PartialView("ConsentSaveResultPartial");
            }
        }
        public async Task<ActionResult> DeleteConsentResultPartial(FormCollection c)
        {
            SaveTransactionResultModel model = new SaveTransactionResultModel();
            //ConsentFormModel model2 = new ConsentFormModel();
            //model2 = await GetconsentID(Guid.Parse(c["consentFormID"]));
            DeleteCustomerAcceptedConsentRequestModel data = new DeleteCustomerAcceptedConsentRequestModel();
            data.accountNo = c["accountNo"];
            data.systemName = "CNMS";
            data.remark = c["remark"];
            data.productName = c["productName"];
            data.userName = Session["usr"].ToString();
            var jsonData = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                var result = await new TokenController().GetToken();

                string token2 = result.Data.ToString();

                // Add Token JWT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);                // Add Token JWT

                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                HttpContent httpContent = new StringContent(jsonData.ToString(), Encoding.UTF8, "application/json");

                // Do the actual request and await the response
                var httpResponse = await client.PostAsync(baseUrl + "/customers/Delete/", httpContent);


                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    try
                    {
                        model = JsonConvert.DeserializeObject<SaveTransactionResultModel>(Response);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                ViewBag.ConsentSaveResultPartial = model;
                return PartialView("ConsentSaveResultPartial");
            }
        }

        #endregion

        #region Test Code
        //{
        //    ViewBag.CustomerList = _ConsentManageServices.GetCustomerAcceptConsent();
        //    ViewBag.Active = "CustomerAcceptedList";
        //    return View();
        //}

        //public ActionResult AcceptedDetailPartial(FormCollection c)
        //{
        //    QueryCustomerAcceptedConsentModel m = new QueryCustomerAcceptedConsentModel();
        //    QueryCustomerConsentDetailRequestModel rm = new QueryCustomerConsentDetailRequestModel();
        //    rm.accountNo = c["accountNo"];
        //    m = _ConsentManageServices.GetCustomerAcceptedConsent(rm);
        //    return PartialView("AcceptedDetailPartial", m);
        //}
        #endregion



    }
}
