using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_Documentmanagement.Application.Interfaces;
using LIB_Documentmanagement.DAL.Entity;
using LIB_Documentmanagement.DAL.Interface;
using LIB_TransactionReversal.DAL.DTO;
using LIB_Usermanagement.DAL.DTO;

namespace LIB_Documentmanagement.Application.Services
{
    public class TransactionReversalService : ITransactionReversalService
    {
        private readonly ITransactionReversalRepository _transactionReversalRepo;
        public TransactionReversalService(ITransactionReversalRepository transactionReversalService)
        {
            _transactionReversalRepo = transactionReversalService;
        }

        public async Task CheckedPendingTransactionForReversal(List<int> transactionReversalIdList)
        {
            try
            {
                await _transactionReversalRepo.CheckedPendingTransactionForReversal(transactionReversalIdList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Response> CreateTransactionReversal(int Id)
        {
            try
            {
               return await _transactionReversalRepo.CreateTransactionReversal(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteTransactionReversal(int Id)
        {
            try
            {
               await _transactionReversalRepo.DeleteTransactionReversal(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionReversal>> GetReversalReport(SearchParams param)
        {
            try
            {
               return await _transactionReversalRepo.GetReversalReport(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TransactionReversal> GetTransactionReversal(int Id)
        {
            try
            {
                return await _transactionReversalRepo.GetTransactionReversal(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionReversal>> GetTransactionReversal(SearchParams param)
        {
            try
            {
                return await _transactionReversalRepo.GetTransactionReversal(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveTransactionReversal(TransactionReversal transactionReversal)
        {
            try
            {
                await _transactionReversalRepo.SaveTransactionReversal(transactionReversal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task updateTransactionReversalStatus(int id, string status, string message, string user="", string transactionId = "")
        {
            try
            {
                await _transactionReversalRepo.updateTransactionReversalStatus(id, status, message, user, transactionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatTransactionReversal(TransactionReversal transactionReversal)
        {
            try
            {
                await _transactionReversalRepo.UpdatTransactionReversal(transactionReversal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
