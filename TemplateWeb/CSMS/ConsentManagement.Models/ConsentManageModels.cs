using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentManament.Models
{
    public class ConsentManageModels
    {

        #region ###############Main Models##################
        public class ConsentFormHeaderModel
        {
            public Guid consentFormID { get; set; }
            public string consentHeader { get; set; }
            public string consentFooter { get; set; }
            public string consentFormName { get; set; }
            public string titleName { get; set; }
            public string remark { get; set; }
            public DateTime ConsentFormDate { get; set; }

        }
        public class QueryCustomerAcceptedConsentModel
        {
            public CustomerAcceptedConsentFormModel consentAcceptedForm { get; set; }
            public List<CustomerAcceptedConsentDetailModel> consentAcceptedDetail { get; set; }
            public string remark { get; set; }

        }
        public class ConsentFormModel
        {
            public Guid consentFormID { get; set; }
            public string consentHeader { get; set; }
            public string consentFooter { get; set; }
            public string consentFormName { get; set; }
            public string titleName { get; set; }
            public List<ConsentModel> consent { get; set; }
            public string remark { get; set; }
        }
        public class DeleteCustomerAcceptedConsentRequestModel
        {
            public string accountNo { get; set; }
            public string productName { get; set; }
            public string userName { get; set; }
            public string systemName { get; set; }
            public string remark { get; set; }

        }
        public class QueryConsentFormRequestModel
        {
            public Guid consentFormId { get; set; }
            public string systemName { get; set; }
            public string systemIpAddress { get; set; }

        }
        public class QueryCustomerConsentDetailRequestModel
        {
            public string accountNo { get; set; }
            public string systemName { get; set; }
            public string systemIpAddress { get; set; }
            //public string maxAddress { get; set; }
        }
        public class AddCustomerAcceptedConsentRequestModel
        {
            public string accountNo { get; set; }
            public Guid consentFormId { get; set; }
            public List<SaveCustomerAcceptedConsentDetailModel> consentAccepted { get; set; }
            public DateTime consentDate { get; set; }
            //public string useStatus { get; set; }
            public string systemName { get; set; }
            public string systemIpAddress { get; set; }
            public string customerIpAddress { get; set; }
            //public string maxAddress { get; set; }
            public string remark { get; set; }

        }
        public class UpdateCustomerAcceptedConsentRequestModel
        {
            public string accountNo { get; set; }
            public string productName { get; set; }
            public Guid consentFormId { get; set; }
            public List<SaveCustomerAcceptedConsentDetailModel> consentAccepted { get; set; }
            public DateTime consentDate { get; set; }
            //public string useStatus { get; set; }
            public string systemName { get; set; }
            public string systemIpAddress { get; set; }
            public string customerIpAddress { get; set; }
            //public string maxAddress { get; set; }
            public string remark { get; set; }

        }

        public class CustomerAcceptedConsentFormModel
        {
            public Guid transactionID { get; set; }
            public string accountNo { get; set; }
            public string productName { get; set; }
            public Guid consentFormID { get; set; }
            public string consentHeader { get; set; }
            public string consentFooter { get; set; }
            public string consentFormName { get; set; }
            public string titleName { get; set; }
            public DateTime consentDate { get; set; }
            //public bool isUse { get; set; }
            //public string systemName { get; set; }
            //public string systemName { get; set; }
            //public string CustomerIpAddress { get; set; }
            //public string maxAddress { get; set; }

        }
        public class ConsentModel
        {
            public Guid consentID { get; set; }
            //public Guid ConsentFormID { get; set; }
            public string titleName { get; set; }
            public Int32 consentOrder { get; set; }
            public string contentHtml { get; set; }
            public string contentText { get; set; }
            public string remark { get; set; }
            public DateTime createDate { get; set; }
            public ConsentStatusModel consentStatus { get; set; }

        }
        public class ConsentStatusModel
        {
            public decimal total { get; set; }
            public decimal accepted { get; set; }
            public decimal notAccepted { get; set; }
            public decimal percent { get; set; }
        }
        public class CustomerAcceptedConsentDetailModel
        {
            //public string AccountNo { get; set; }
            public ConsentModel Consent { get; set; }
            public bool IsAccept { get; set; }
            //public DateTime DateTime { get; set; }
        }
        public class SaveCustomerAcceptedConsentDetailModel
        {
            //public string accountNo { get; set; }
            public Guid consentId { get; set; }
            public bool isAccepted { get; set; }
        }
        public class ValidConsentFormModel
        {
            //public Guid consentFormID { get; set; }
            public bool valid { get; set; }
            public string remark { get; set; }
        }
        public class ValidCustomerAcceptedConsentModel
        {
            public Guid consentFormID { get; set; }
            public string accountNo { get; set; }
            public bool valid { get; set; }
            public string remark { get; set; }
        }
        public class SaveTransactionResultModel
        {
            public bool isSuccess { get; set; }
            public int statusCode { get; set; }// แล้วแต่จะตกลงกัน
            public string remark { get; set; }
        }
        #endregion

        #region ############### Web Models #####################
        public class ShowCustomerAcceptedConsent
        {
            public DateTime transactionDate { get; set; }
            public string accountNo { get; set; }
            public string refNo { get; set; }
            public string productName { get; set; }
            public string customerName { get; set; }
            public string consentFormName { get; set; }
            public string status { get; set; }
        }
        public class SearchCustomerAcceptedConsentRequestModel
        {
            public string accountNo { get; set; }
            public string productName { get; set; }
            public string consentFormName { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
        }
        public class ShowDashBoard1 {
            public decimal total { get; set; }
            public decimal fullAccepted { get; set; }
            public decimal partialAccepted { get; set; }
            public decimal notAccepted { get; set; }
        }

        public class ShowDashBoard2
        {
            public List<ConsentFormModel> consentForm { get; set; }
        }
        #endregion


    }
}
