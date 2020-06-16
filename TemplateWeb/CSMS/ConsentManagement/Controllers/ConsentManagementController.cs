using ConsentManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using static ConsentManament.Models.ConsentManageModels;

namespace ConsentManament.Controllers
{
    [AspAuthen]
    public class ConsentManagementController : Controller
    {
        string baseUrl = ConfigurationManager.AppSettings["Baseurl"].ToString();
        string systemName = ConfigurationManager.AppSettings["SystemName"].ToString();
        string systemIpAddress = ConfigurationManager.AppSettings["SystemIpAddress"].ToString();
        // GET: ConsentManagement
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Current timeout: " + Session.Timeout);
                System.Diagnostics.Debug.WriteLine("Session timeout: " + Session["timeoutDate"]);
            return View();
        }

        public async Task<ActionResult> ShowConsentDetailPartial(FormCollection c)
        {
            ConsentFormModel model = new ConsentFormModel();

            using (var client = new HttpClient())
            {
                JObject JsonData = new JObject(
                        new JProperty("consentFormId", c["consent_form_id"]),
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
                var httpResponse = await client.GetAsync(baseUrl + "/forms/"+c["consent_form_id"]);


                if (httpResponse.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    model = JsonConvert.DeserializeObject<ConsentFormModel>(Response);
                }
                ViewBag.ConsentDetail = model;
                return PartialView("ShowConsentDetailPartial");
            }
        }
        public async Task<ActionResult> ConsentForm()
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

                ViewBag.ConsentFormList = model;
                return View("ConsentFormList",model);
            }
        }
    }
}
