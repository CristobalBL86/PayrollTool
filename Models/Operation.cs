using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class Operation
    {
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public string Description { get; set; }

        public ICollection<Price> Prices { get; set; }
        public ICollection<TransactionLog> Transactions { get; set; }
    }
}
