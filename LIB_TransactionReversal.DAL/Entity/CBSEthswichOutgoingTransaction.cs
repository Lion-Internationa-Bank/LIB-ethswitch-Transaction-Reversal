using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.Entity
{
    [Table("cbs_ethswich_outgoing_transaction")]
    public class CBSEthswichOutgoingTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string branch { get; set; }
        public string operation_code { get; set; }
        public string cust_id { get; set; }
        public string account_debited { get; set; }
        public decimal amount { get; set; }
        public string side1 { get; set; }
        public string creditor_branch { get; set; }
        public string account_credited { get; set; }
        public string name_creditor { get; set; }
        public string side2 { get; set; }
        public string ref_no { get; set; }
        public string status { get; set; }
        public DateTime date_1 { get; set; }
        public string time_1 { get; set; }
        public DateTime? importedDate { get; set; }

        [NotMapped]
        public string TransactionType { get; set; } = "0";

        [NotMapped]
        public string BankName { get; set; }
    }
}
