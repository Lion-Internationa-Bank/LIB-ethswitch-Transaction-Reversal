using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class ImportEthswichTransactionDto
    {
        public string Acquirer { get; set; }
        public decimal Amount { get; set; }
        public string Authidresp_F38 { get; set; }
        public string Bo_utrnno { get; set; }
        public string Card_Number { get; set; }
        public string Currency { get; set; }
        public string Fe_utrnno { get; set; }
        public bool IsAlreadyExist { get; set; }
        public string Issuer { get; set; }
        public string MTI { get; set; }
        public string Refnum_F37 { get; set; }
        public string STAN_F11 { get; set; }
        public string Terminal_ID { get; set; }
        public string Transaction_Date { get; set; }
        public string Transaction_Description { get; set; }
        public string Transaction_Place { get; set; }
        public string Status { get; set; }
        public string TransactionDateFrom { get; set; }
        public string TransactionDateTo { get; set; }
    }
}
