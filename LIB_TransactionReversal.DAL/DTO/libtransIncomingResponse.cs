using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class libtransIncomingResponse
    {
        public bool success { get; set; }
        public List<LIBIncommingTransactionResponse> transactions { get; set; }
    }

    public class LIBIncommingTransactionResponse
    {
        public string id { get; set; }
        public string requestId { get; set; }
        public decimal amount { get; set; }
        public string branch { get; set; }
        public string account { get; set; }
        public string transferRefNo { get; set; }
        public string ethswitchRefNo { get; set; }
        public string statusEthswitch { get; set; }
        public string statusTransfer { get; set; }
        public DateTime timeStamp { get; set; }
        public string name { get; set; }
    }
}
