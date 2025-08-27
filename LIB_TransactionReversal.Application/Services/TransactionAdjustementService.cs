using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_TransactionReversal.Application.Interfaces;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.DTO;
using LIB_Usermanagement.DAL.Entity.Account;

namespace LIB_TransactionReversal.Application.Services
{
    public class TransactionAdjustementService : ITransactionAdjustementService
    {
        private readonly ITransactionAdjustementRepository _transactionAdjustementRepository;
        public TransactionAdjustementService(ITransactionAdjustementRepository transactionAdjustementRepository) 
        {
            _transactionAdjustementRepository = transactionAdjustementRepository;
        }
        public Task CheckedPendingTransactionForReversal(List<int> transactionAdjustementIdList)
        {
            try
            {
                return _transactionAdjustementRepository.CheckedPendingTransactionForReversal(transactionAdjustementIdList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking pending transactions for reversal", ex);
            }
        }

        public Task<Response> CreateTransactionReversal(int Id)
        {
            try
            {
                return _transactionAdjustementRepository.CreateTransactionReversal(Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating transaction reversal", ex);
            }
        }

        public Task DeleteTransactionAdjustement(int Id)
        {
            try
            {
                return _transactionAdjustementRepository.DeleteTransactionAdjustement(Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting transaction adjustment", ex);
            }
        }

        public Task<List<TransactionAdjustement>> GetAdjustementReport(SearchParams param)
        {
            try
            {
                return _transactionAdjustementRepository.GetAdjustementReport(param);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting adjustment report", ex);
            }
        }
        public Task<TransactionAdjustement> GetTransactionAdjustement(int Id)
        {
            try
            {
                return _transactionAdjustementRepository.GetTransactionAdjustement(Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting transaction adjustment", ex);
            }
        }

        public Task<List<TransactionAdjustement>> GetTransactionAdjustement(SearchParams param)
        {
            try
            {
                return _transactionAdjustementRepository.GetTransactionAdjustement(param);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting transaction adjustments", ex);
            }
        }

        public Task SaveTransactionAdjustement(TransactionAdjustement transactionAdjustement)
        {
            try
            {
                return _transactionAdjustementRepository.SaveTransactionAdjustement(transactionAdjustement);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving transaction adjustment", ex);
            }
        }

        public Task SaveTransactionAdjustement(List<TransactionAdjustement> transactionAdjustementList, TrasactionReversalDbContext context)
        {
            try
            {
                return _transactionAdjustementRepository.SaveTransactionAdjustement(transactionAdjustementList, context);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving transaction adjustments", ex);
            }
        }

        public Task updateTransactionAccountNumber(UpdateTransactionAccountDto objTranAdjustement)
        {
            try
            {
                return _transactionAdjustementRepository.updateTransactionAccountNumber(objTranAdjustement);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating transaction adjustment status", ex);
            }
        }

        public Task updateTransactionAdjustementStatus(int id, string status, string message, string user = "", string transactionId = "")
        {
            try
            {
                return _transactionAdjustementRepository.updateTransactionAdjustementStatus(id, status, message, user, transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating transaction adjustment status", ex);
            }
        }

        public async Task UpdatTransactionAdjustement(TransactionAdjustement transactionAdjustement)
        {
            try
            {
                 await _transactionAdjustementRepository.UpdatTransactionAdjustement(transactionAdjustement);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating transaction adjustment", ex);
            }
        }
    }
}
