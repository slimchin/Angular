using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentManagement.Models.UsersManagement
{
    public class UserModel
    {
        public string ID { get; set; }
        public int Userno { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Status { get; set; }
        
    }
    public class ActivityModel
    {
        public string ID { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Activity { get; set; }
        public string Remark { get; set; }
        public DateTime LogDatetime { get; set; }

    }
    
}
