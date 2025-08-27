using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.Entity
{
    [Table("transaction_notfound_at_ethSwitch")]
    public class TransactionNotFoundAtEthSwitch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DebitedAccountNumber { get; set; }
        public string CreditedAccount { get; set; }
        public decimal Amount { get; set; }
        public string Branch { get; set; }
        public string RefNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string TransactionType { get; set; }
        public string BankName { get; set; }
        public string Reason { get; set; }
    }
}
