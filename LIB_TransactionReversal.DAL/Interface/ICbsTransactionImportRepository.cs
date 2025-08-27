using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;

namespace LIB_TransactionReversal.DAL.Interface
{
    public interface ICbsTransactionImportRepository
    {
        Task ImportCbsOutgoingTransaction();
        Task ImportCbsIncomingTransaction();
        Task ImportCbsOutgoingTransaction(DateTime transactionDate);
        Task ImportCbsIncomingTransaction(DateTime transactionDate);
        Task<List<AccountBranch>> GetAccountBranch(List<string> accountNos);
        Task<AccountBranch> GetAccountDetail(string accountNo);

        Task<List<TransactionValidation>> ValidTransactionSuccess(string transactionId);
        Task ImportPendingCbsIncomingTransaction();
    }
}
