using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class ReconsillationSummaryReportDto
    {
        public string TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public int PendingOnLIBCount { get; set; }
        public int PendingOnEthswichCount { get; set; }
        public int SuccessfullyReconsiledCount { get; set; }

    }
}
