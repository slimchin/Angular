using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentManagement.Models
{
    public class MenuPermissionModel
    {
        public string ProgramCode { get; set; }
        public string MenuCode { get; set; }
        public string MenuDescription { get; set; }
        public string MenuIndex { get; set; }
        public IEnumerable<ChildMenuModel> ChildMenuModel { get; set; }
    }
    public class ChildMenuModel
    {
        public string ProgramCode { get; set; }
        public string MenuCode { get; set; }
        public string MenuDescription { get; set; }
        public string MenuIndex { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
