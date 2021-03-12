using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string NickName { get; set; }
        public string Name {
            //get{
            //    return $"{this.Name} {this.LastName}";
            //}
            //set { }
            get; set;
        }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public bool Status { get; set; }
        public ICollection<AssistanceLog> Assistances { get; set; }
    }
}