using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int PalletBoxQty { get; set; }


        public ICollection<Price> Prices { get; set; }
        public ICollection<TransactionLog> Transactions { get; set; }
    }
}
