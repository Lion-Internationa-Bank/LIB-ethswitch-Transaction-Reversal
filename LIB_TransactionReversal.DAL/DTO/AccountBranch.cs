using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class AccountBranch
    {
        [Column("ACCOUNTNUMBER")]
        public string accountnumber { get; set; }
        [Column("BRANCH")]
        public string branch { get; set; }
        public string FULLNAME { get; set; }
        
    }
}
