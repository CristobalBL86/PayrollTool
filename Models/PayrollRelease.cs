using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class PayrollRelease
    {
        [Display(Name ="ID")]
        public int PayrollReleaseId { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        [Display(Name ="Employee")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        [Range(1,int.MaxValue, ErrorMessage = "Ammount must be greater than zero 0")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public float Ammount { get; set; }
        public Employee Employee { get; set; }
    }
}
