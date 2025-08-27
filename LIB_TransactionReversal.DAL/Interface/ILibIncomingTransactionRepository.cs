using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.Interface
{
    public interface ILibIncomingTransactionRepository
    {
        Task GetBatchEthswitchIncommingTransaction();
    }
}
