using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class TransactionLog
    {
        [Display(Name ="ID")]
        public int TransactionLogId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "This Field is required.")]
        public DateTime Date { get; set; }
        [Display(Name = "Product")]
        [Required(ErrorMessage = "This Field is required.")]
        public int ProductId { get; set; }
        [Display(Name = "Operation")]
        [Required(ErrorMessage = "This Field is required.")]
        public int OperationId { get; set; }
        [Display(Name = "Box Qty")]
        [Required(ErrorMessage = "This Field is required.")]
        [Range(1,int.MaxValue, ErrorMessage ="Box Qty must be greater than zero 0.")]
        public int BoxQty { get; set; }
        public Product Product { get; set; }
        public Operation Operation { get; set; }
    }
}
