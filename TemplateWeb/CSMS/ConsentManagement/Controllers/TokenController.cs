//using DataSubjectRight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Configuration;
using ConsentManagement.Models;
using ConsentManament.Models;

namespace ConsentManament.Controllers
{
    public class TokenController : Controller
    {
        // GET: Token
        string Baseurl = ConfigurationManager.AppSettings["GetToken"].ToString();
        string ClientCode = ConfigurationManager.AppSettings["ClientCode"].ToString();
        string JWTRequestUserName = ConfigurationManager.AppSettings["JWTRequestUserName"].ToString();
        string JWTRequestPassword = ConfigurationManager.AppSettings["JWTRequestPassword"].ToString();       
        
        public async Task<JsonResult> GetToken()
        {
            using (var client = new HttpClient())
            {
     
                JObject JsonData = new JObject(
                        new JProperty("ClientCode", ClientCode),
                        new JProperty("JWTRequestUserName", JWTRequestUserName),
                        new JProperty("JWTRequestPassword", JWTRequestPassword)
                    );

                HttpContent httpContent = new StringContent(JsonData.ToString(), Encoding.UTF8, "application/json");

                var httpResponse = await client.PostAsync(Baseurl, httpContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var Response = httpResponse.Content.ReadAsStringAsync().Result;

                    TokenModel token = await httpResponse.Content.ReadAsAsync<TokenModel>();

                    return Json(token.jwtToken.ToString());
                }
                else
                {
                    return Json("");
                }
            }
        }

    }
}