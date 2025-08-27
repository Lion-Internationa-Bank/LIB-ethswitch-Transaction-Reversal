using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Documentmanagement.DAL.Entity
{
    [Table("transaction_reversal")]
    public class TransactionReversal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DebitedAccountNumber { get; set; }
        public string CreditedAccount { get; set; }
        public Decimal Amount { get; set; }
        public string RefNo { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? VAT { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime createdAt { get; set; } 
        public string ApprovedBy { get; set; } 
        public DateTime? ApprovedDate { get; set; }
        public string ReversedBy { get; set; }
        public DateTime? ReversalDate { get; set; }
        public string branch { get; set; }
        public string Message { get; set; }
        public string newTransaction { get; set; }
    }
}
