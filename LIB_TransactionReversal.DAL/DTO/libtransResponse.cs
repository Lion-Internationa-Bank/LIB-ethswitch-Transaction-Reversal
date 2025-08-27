using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class libtransResponse
    {
        public bool success { get; set; }
        public List<LIBTransaction> transactions { get; set; }
    }

    public class LIBTransaction
    {
        public string _id { get; set; }
        public string accountNumber { get; set; }
        public string receiverAccount { get; set; }
        public decimal amount { get; set; }
        public string branch { get; set; }
        public string rrn { get; set; }
        public string statusEthswitch { get; set; }
        public string statusTransfer { get; set; }
        public DateTime createdAt { get; set; }
        public string bankName { get; set; }
    }
}
