using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.Entity
{
    [Table("Lib_outgoing_transaction")]
    public class LibOutgoingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string AccountNumber { get; set; }
        public string ReceiverAccount { get; set; }
        public decimal Amount { get; set; }
        public string Branch { get; set; }
        
        public string Rrn { get; set; }
        public string StatusEthswitch { get; set; }
        public string StatusTransfer { get; set; }
        public string BankName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ImportedDate { get; set; }  
    }
}
