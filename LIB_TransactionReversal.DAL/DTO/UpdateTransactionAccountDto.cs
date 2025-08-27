using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class UpdateTransactionAccountDto
    {
        public int Id { get; set; }
        public string CreditedAccount { get; set; }
        public string RefNo { get; set; }
        public string CustomerName { get; set; }
    }
}
