using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.Entity
{
    [Table("ETHSWITCH_IN")]
    public class TransactionValidation
    {
        [Column("REF_NO")]
        public string refNo { get; set; }
        [Column("ACCOUNT_CREDITED")]
        public string accountCredited { get; set; }
        [Column("CREDITOR_BRANCH")]
        public string creditorBranch { get; set; }
    }

}
