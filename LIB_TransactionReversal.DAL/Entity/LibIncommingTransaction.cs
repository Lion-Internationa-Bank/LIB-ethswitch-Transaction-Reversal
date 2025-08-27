using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.Entity
{
    [Table("Lib_Incomming_Transaction")]
    public class LibIncommingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Branch { get; set; }
        public string Account { get; set; }
        public string TransferRefNo { get; set; }
        public string EthswitchRefNo { get; set; }
        public string StatusEthswitch { get; set; }
        public string StatusTransfer { get; set; }
        public DateTime TimeStamp { get; set; }
        public string BankName { get; set; }
        public DateTime ImportedDate { get; set; }
    }
}
