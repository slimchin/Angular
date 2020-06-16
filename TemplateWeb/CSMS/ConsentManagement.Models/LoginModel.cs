using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsentManament.Models
{
    public class LoginStatusModel
    {
        public int LoginStatus { get; set; }
        public string LoginDetail { get; set; }
    }
    public class UserDbModel
    {
        public int Id { get; set; }
        public string UserFirstName { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
    }
    public class TokenModel
    {
        public string jwtTokenID { get; set; }
        public string jwtToken { get; set; }
        public string currentDateTime { get; set; }
        public string expiredDateTime { get; set; }
    }

    public class ADUser
    {
        public string sAMAccountName { get; set; }
        public string employeeID { get; set; }
        public string DisplayName { get; set; }
        public string departmentNumber { get; set; }
        public string department { get; set; }
        public string Email { get; set; }
        //public string givenName { get; set; }
    }

    //public class TokenModel
    //{
    //    public string ClientCode { get; set; }
    //    public string JWTRequestUserName { get; set; }
    //    public string JWTRequestPassword { get; set; }
    //}

}
