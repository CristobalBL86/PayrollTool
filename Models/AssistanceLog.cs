using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class AssistanceLog
    {
        [Display(Name ="ID")]
        public int AssistanceLogId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Display(Name ="Employee")]
        [Required(ErrorMessage = "This Field is required.")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
