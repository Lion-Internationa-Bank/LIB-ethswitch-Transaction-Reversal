using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_Documentmanagement.DAL.Entity;
using LIB_TransactionReversal.DAL.DTO;
using LIB_Usermanagement.DAL.DTO;

namespace LIB_Documentmanagement.Application.Interfaces
{
    public interface ITransactionReversalService
    {
        Task SaveTransactionReversal(TransactionReversal transactionReversal);
        Task DeleteTransactionReversal(int Id);
        Task<TransactionReversal> GetTransactionReversal(int Id);
        Task<List<TransactionReversal>> GetTransactionReversal(SearchParams param);
        Task<List<TransactionReversal>> GetReversalReport(SearchParams param);
        Task UpdatTransactionReversal(TransactionReversal transactionReversal);
        Task updateTransactionReversalStatus(int id, string status, string message, string user="", string transactionId ="");
        Task CheckedPendingTransactionForReversal(List<int> transactionReversalIdList);
        Task<Response> CreateTransactionReversal(int Id);
    }
}
