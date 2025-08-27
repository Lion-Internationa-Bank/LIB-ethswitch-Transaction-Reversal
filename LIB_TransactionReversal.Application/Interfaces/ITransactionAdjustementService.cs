using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_TransactionReversal.DAL.Entity;
using LIB_Usermanagement.DAL.DTO;
using LIB_Usermanagement.DAL;
using LIB_TransactionReversal.DAL.DTO;

namespace LIB_TransactionReversal.Application.Interfaces
{
    public interface ITransactionAdjustementService
    {
        Task SaveTransactionAdjustement(TransactionAdjustement transactionAdjustement);
        Task DeleteTransactionAdjustement(int Id);
        Task<TransactionAdjustement> GetTransactionAdjustement(int Id);
        Task<List<TransactionAdjustement>> GetTransactionAdjustement(SearchParams param);
        Task<List<TransactionAdjustement>> GetAdjustementReport(SearchParams param);
        Task UpdatTransactionAdjustement(TransactionAdjustement transactionAdjustement);
        Task updateTransactionAdjustementStatus(int id, string status, string message, string user = "", string transactionId = "");
        Task SaveTransactionAdjustement(List<TransactionAdjustement> transactionAdjustementList, TrasactionReversalDbContext context);
        Task CheckedPendingTransactionForReversal(List<int> transactionAdjustementIdList);
        Task<Response> CreateTransactionReversal(int Id);
        Task updateTransactionAccountNumber(UpdateTransactionAccountDto objTranAdjustement);
    }
}
