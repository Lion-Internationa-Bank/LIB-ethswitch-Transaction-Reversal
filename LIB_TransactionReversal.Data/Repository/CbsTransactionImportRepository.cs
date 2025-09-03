using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LIB_TransactionReversal.DAL.Contexts;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace LIB_TransactionReversal.Infra.Data.Repository
{
    public class CbsTransactionImportRepository : ICbsTransactionImportRepository
    {
        private readonly TrasactionReversalDbContext _context;
        private readonly CBSDbContext _cbsContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public CbsTransactionImportRepository(TrasactionReversalDbContext context, IHttpContextAccessor httpContextAccessor,
            IMapper mapper, CBSDbContext cbsContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _cbsContext = cbsContext;
        }

        public async Task ImportCbsOutgoingTransaction()
        {
            try
            {
                //var cuurentDate = DateTime.Today.AddDays(-1).ToString("dd-MMM-yy");
                for (int i = 1; i <= 3; i++)
                {
                    var cuurentDate = DateTime.Today.AddDays(-i).ToString("dd-MMM-yy");
                    //var cuurentDate = new DateTime(2025, 5, 14).ToString("dd-MMM-yy");
                    var CbsOutgoingTransactions = await _cbsContext.CBSEthswichOutgoingTransaction.FromSqlRaw(@$"select '' as Id ,branch,operation_code,cust_id,account_debited,amount,side1, creditor_branch, 
                   account_credited,name_creditor, side2, ref_no, status, date_1, time_1, SYSDATE  as importedDate from anbesaprod.ethswitch_outgoing where date_1='{cuurentDate}'", "").ToListAsync();
                    var RefwithCompleteTransaction = CbsOutgoingTransactions.GroupBy(p => p.ref_no).Where(g => g.Count() == 3).Select(g => g.Key.Trim()).ToList();
                    CbsOutgoingTransactions = CbsOutgoingTransactions.Where(p => RefwithCompleteTransaction.Contains(p.ref_no.Trim())).ToList();
                    foreach (var item in CbsOutgoingTransactions)
                    {
                        item.ref_no = item.ref_no.Trim();
                        item.name_creditor = item.name_creditor.Trim();
                    }
                    List<decimal> amount = CbsOutgoingTransactions.Select(p => p.amount).ToList();
                    List<string> refenceNo = CbsOutgoingTransactions.Select(p => p.ref_no).ToList();
                    List<DateTime> date = CbsOutgoingTransactions.Select(p => p.date_1.Date).ToList();
                    List<string> time = CbsOutgoingTransactions.Select(p => p.time_1).ToList();
                    var exist = _context.CBSEthswichOutgoingTransaction
                        .Where(p => amount.Contains(p.amount) && refenceNo.Contains(p.ref_no) && date.Contains(p.date_1.Date) && time.Contains(p.time_1))
                        .ToList();
                    if (exist.Count > 0)
                    {
                        CbsOutgoingTransactions.RemoveAll(p => exist.Any(e => e.amount == p.amount && e.ref_no == p.ref_no && e.date_1.Date == p.date_1.Date && e.time_1 == p.time_1));
                    }

                    _context.CBSEthswichOutgoingTransaction.AddRange(CbsOutgoingTransactions);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ImportCbsIncomingTransaction()
        {
            try
            {
                //var cuurentDate = DateTime.Today.AddDays(-1).ToString("dd-MMM-yy");
                for (int i = 1; i <= 3; i++)
                {
                    var cuurentDate = DateTime.Today.AddDays(-i).ToString("dd-MMM-yy");
                    var CbsIncomingTransactions = await _cbsContext.CBSEthswichIncomingTransaction.FromSqlRaw(@$"select '' as Id ,branch,operation_code,cust_id,account_debited,amount,side1, creditor_branch, 
                   account_credited,name_creditor, side2, ref_no, status, date_1, time_1, SYSDATE  as importedDate from anbesaprod.Ethswitch_incoming where date_1='{cuurentDate}'", "").ToListAsync();
                    foreach (var item in CbsIncomingTransactions)
                    {
                        item.ref_no = item.ref_no.Trim();
                        item.name_creditor = item.name_creditor.Trim();
                    }
                    List<decimal> amount = CbsIncomingTransactions.Select(p => p.amount).ToList();
                    List<string> refenceNo = CbsIncomingTransactions.Select(p => p.ref_no).ToList();
                    List<DateTime> date = CbsIncomingTransactions.Select(p => p.date_1.Date).ToList();
                    List<string> time = CbsIncomingTransactions.Select(p => p.time_1).ToList();
                    var exist = _context.CBSEthswichIncomingTransaction
                        .Where(p => amount.Contains(p.amount) && refenceNo.Contains(p.ref_no) && date.Contains(p.date_1.Date) && time.Contains(p.time_1))
                        .ToList();
                    if (exist.Count > 0)
                    {
                        CbsIncomingTransactions.RemoveAll(p => exist.Any(e => e.amount == p.amount && e.ref_no == p.ref_no && e.date_1.Date == p.date_1.Date && e.time_1 == p.time_1));
                    }
                    _context.CBSEthswichIncomingTransaction.AddRange(CbsIncomingTransactions);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task ImportCbsOutgoingTransaction(DateTime transactionDate)
        {
            try
            {
                if (transactionDate.Date == DateTime.Now.Date)
                {
                    throw new Exception("You cannot import today's transactions. Please try again tomorrow.");
                }
                var cuurentDate = transactionDate.ToString("dd-MMM-yy");
                var CbsOutgoingTransactions = await _cbsContext.CBSEthswichOutgoingTransaction.FromSqlRaw(@$"select '' as Id ,branch,operation_code,cust_id,account_debited,amount,side1, creditor_branch, 
                   account_credited,name_creditor, side2, ref_no, status, date_1, time_1, SYSDATE  as importedDate from anbesaprod.ethswitch_outgoing where date_1='{cuurentDate}'", "").ToListAsync();
                var RefwithCompleteTransaction = CbsOutgoingTransactions.GroupBy(p => p.ref_no).Where(g => g.Count() == 3).Select(g => g.Key.Trim()).ToList();
                CbsOutgoingTransactions = CbsOutgoingTransactions.Where(p => RefwithCompleteTransaction.Contains(p.ref_no.Trim())).ToList();
                foreach (var item in CbsOutgoingTransactions)
                {
                    item.ref_no = item.ref_no.Trim();
                    item.name_creditor = item.name_creditor.Trim();
                }
                List<decimal> amount = CbsOutgoingTransactions.Select(p => p.amount).ToList();
                List<string> refenceNo = CbsOutgoingTransactions.Select(p => p.ref_no).ToList();
                List<DateTime> date = CbsOutgoingTransactions.Select(p => p.date_1.Date).ToList();
                List<string> time = CbsOutgoingTransactions.Select(p => p.time_1).ToList();
                var exist = _context.CBSEthswichOutgoingTransaction
                    .Where(p => amount.Contains(p.amount) && refenceNo.Contains(p.ref_no) && date.Contains(p.date_1.Date) && time.Contains(p.time_1))
                    .ToList();
                if (exist.Count > 0)
                {
                    CbsOutgoingTransactions.RemoveAll(p => exist.Any(e => e.amount == p.amount && e.ref_no == p.ref_no && e.date_1.Date == p.date_1.Date && e.time_1 == p.time_1));
                }

                _context.CBSEthswichOutgoingTransaction.AddRange(CbsOutgoingTransactions);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ImportCbsIncomingTransaction(DateTime transactionDate)
        {
            try
            {
                if(transactionDate.Date == DateTime.Now.Date)
                {
                    throw new Exception("You cannot import today's transactions. Please try again tomorrow.");
                }
                var cuurentDate = transactionDate.ToString("dd-MMM-yy");
                var CbsIncomingTransactions = await _cbsContext.CBSEthswichIncomingTransaction.FromSqlRaw(@$"select '' as Id ,branch,operation_code,cust_id,account_debited,amount,side1, creditor_branch, 
                   account_credited,name_creditor, side2, ref_no, status, date_1, time_1, SYSDATE  as importedDate from anbesaprod.Ethswitch_incoming where date_1='{cuurentDate}'", "").ToListAsync();
                foreach (var item in CbsIncomingTransactions)
                {
                    item.ref_no = item.ref_no.Trim();
                    item.name_creditor = item.name_creditor.Trim();
                }
                List<decimal> amount = CbsIncomingTransactions.Select(p => p.amount).ToList();
                List<string> refenceNo = CbsIncomingTransactions.Select(p => p.ref_no).ToList();
                List<DateTime> date = CbsIncomingTransactions.Select(p => p.date_1.Date).ToList();
                List<string> time = CbsIncomingTransactions.Select(p => p.time_1).ToList();
                var exist = _context.CBSEthswichIncomingTransaction
                    .Where(p => amount.Contains(p.amount) && refenceNo.Contains(p.ref_no) && date.Contains(p.date_1.Date) && time.Contains(p.time_1))
                    .ToList();
                if (exist.Count > 0)
                {
                    CbsIncomingTransactions.RemoveAll(p => exist.Any(e => e.amount == p.amount && e.ref_no == p.ref_no && e.date_1.Date == p.date_1.Date && e.time_1 == p.time_1));
                }
                _context.CBSEthswichIncomingTransaction.AddRange(CbsIncomingTransactions);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ImportPendingCbsIncomingTransaction()
        {
            try
            {
                DateTime transactionDate = DateTime.Now;
               
                var cuurentDate = transactionDate.ToString("dd-MMM-yy");

                var CbsIncomingTransactions = await _cbsContext.CBSPendingEthswichIncomingTransaction.FromSqlRaw(@$"select '' as Id ,branch,operation_code,cust_id,account_debited,amount,side1, creditor_branch, 
                   account_credited,name_creditor, side2, ref_no, status, date_1, time_1, SYSDATE  as importedDate from anbesaprod.Ethswitch_incoming where  status != 'VA' and status != 'VF' and date_1='{cuurentDate}'", "").ToListAsync();

                _context.CBSPendingEthswichIncomingTransaction.AddRange(CbsIncomingTransactions);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<AccountBranch>> GetAccountBranch(List<string> accountNos)
        {
            try
            {
                var accounts = string.Join(",", accountNos);
                var parameterList = string.Join(",", accountNos.Select((id, index) => $"@p{index}"));
                var sql = $"select accountnumber,branch,fullname from anbesaprod.CUSTOMER_INFO_2 where accountnumber IN ({accounts})";
                var parameters = accountNos.Select((id, index) => new OracleParameter($":p{index}", id)).ToArray();
                return await _cbsContext.AccountBranch.FromSqlRaw(sql, parameters)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AccountBranch> GetAccountDetail(string accountNo)
        {
            try
            {
                var parameterList = accountNo;
                var sql = $"select accountnumber,branch,FULLNAME from anbesaprod.CUSTOMER_INFO_2 where accountnumber = '{accountNo}'";
                var parameters =  new OracleParameter($":p1", accountNo);
                return await _cbsContext.AccountBranch.FromSqlRaw(sql, parameters)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionValidation>> ValidTransactionSuccess(string transactionId)
        {
            try
            {
                
               // var sql = $"SELECT  ref_no, account_credited, creditor_branch FROM anbesatest3.ETHSWITCH_INL WHERE ref_no='{transactionId}'";
                //var parameters = accountNos.Select((id, index) => new OracleParameter($":p{index}", id)).ToArray();

                var sql = "SELECT ref_no, account_credited, creditor_branch FROM anbesatest3.ETHSWITCH_INL1 WHERE ref_no = :transactionId";
                var parameter = new OracleParameter("transactionId", transactionId);
                var response = new List<TransactionValidation>();
                response = await _cbsContext.TransactionValidation.FromSqlRaw(sql, transactionId)
                        .ToListAsync();

                return response;
            }
            catch (Exception ex)
            {
                return new List<TransactionValidation>();
            }
        }
    }
}
