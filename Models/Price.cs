using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public int ProductId { get; set; }
        public int OperationId { get; set; }
        public float UnitPrice { get; set; }
        public Product Product { get; set; }
        public Operation Operation { get; set; }
    }
}
