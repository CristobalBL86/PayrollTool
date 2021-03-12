using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class TransactionLogView
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Product { get; set; }
        public string Operation { get; set; }
        public int BoxQty { get; set; }
    }
}
