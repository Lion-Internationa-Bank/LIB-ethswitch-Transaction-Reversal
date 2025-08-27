using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LIB_Documentmanagement.DAL.Entity;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LIB_TransactionReversal.Infra.Data.Repository
{
    public class EthswitchTransactionImportRepository : IEthswitchTransactionImportRepository
    {
        private readonly TrasactionReversalDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICbsTransactionImportRepository _iCbsTransactionImportRepo;
        public EthswitchTransactionImportRepository(TrasactionReversalDbContext context, IHttpContextAccessor httpContextAccessor,
            IMapper mapper, ICbsTransactionImportRepository iCbsTransactionImportRepo)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _iCbsTransactionImportRepo = iCbsTransactionImportRepo;
        }

        public async Task ImportEthswitchTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport)
        {
            try
            {
                //ethswitchTransactionImport.ForEach(p =>
                //{
                //    p.cretedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                //    p.CreatedDate = DateTime.Now;
                //});
                ethswitchTransactionImport.RemoveAll(p => p == null);
                var OutgoingTransaction = _mapper.Map<List<EthswitchOutgoingTransactionImport>>(ethswitchTransactionImport.Where(p => p.Issuer == "Lion Bank A2A").ToList());
                await _context.EthswitchOutgoingTransactionImport.AddRangeAsync(OutgoingTransaction);
                var IncommingTransaction = _mapper.Map<List<EthswitchIncommingTransactionImport>>(ethswitchTransactionImport.Where(p => p.Issuer != "Lion Bank A2A").ToList());
                await _context.EthswitchIncommingTransactionImport.AddRangeAsync(IncommingTransaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ImportEthswichTransactionDto>> CheckTransactionExist(List<ImportEthswichTransactionDto> ethswitchTransactionImport)
        {
            try
            {
                ethswitchTransactionImport.RemoveAll(p => p == null);
                var OutgoingTransaction = _mapper.Map<List<EthswitchOutgoingTransactionImport>>(ethswitchTransactionImport.Where(p => p.Issuer == "Lion Bank A2A").ToList());
                var reference = OutgoingTransaction.Select(p => p.Refnum_F37).ToList();
                var existingTransaction = await _context.EthswitchOutgoingTransactionImport.Where(p => reference.Contains(p.Refnum_F37)).ToListAsync();
                return _mapper.Map<List<ImportEthswichTransactionDto>>(existingTransaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ImportEthswichTransactionDto>> InsertInvalidDateEthiswichTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport)
        {
            try
            {
                ethswitchTransactionImport.RemoveAll(p => p == null);
                var InvalidDateTransaction = _mapper.Map<List<EthswitchInvalidDateTransaction>>(ethswitchTransactionImport);
                await _context.EthswitchInvalidDateTransaction.AddRangeAsync(InvalidDateTransaction);
                await _context.SaveChangesAsync();
                return ethswitchTransactionImport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EthswitchInvalidDateTransaction>> GetInvalidEthiswichDateTransaction(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);

                if (param.Date == new DateTime())
                {
                    param.Date = DateTime.Today;
                }
                return await _context.EthswitchInvalidDateTransaction.Where(p =>
                p.ExcelDate.Date == param.Date.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EthswitchOutgoingTransactionImport>> CheckTransactionRangeExist(DateTime from, DateTime to)
        {
            try
            {
                // to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);
                var existingTransaction = await _context.EthswitchOutgoingTransactionImport.Where(p => p.Transaction_Date.Date == from.Date).ToListAsync();
                return existingTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task TransactionNotFoundEthswitch(List<TransactionNotFoundAtEthSwitch> transactionNotFoundAtEthSwitch, TrasactionReversalDbContext context)
        {
            try
            {
                //transactionNotFoundAtEthSwitch.ForEach(p =>
                //{
                //    p.createdAt = DateTime.Now;
                //});
                await context.TransactionNotFoundAtEthSwitch.AddRangeAsync(transactionNotFoundAtEthSwitch);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SuccessfullTransaction(List<SuccessfullTransaction> successfullTransaction, TrasactionReversalDbContext context)
        {
            try
            {
                //successfullTransaction.ForEach(p =>
                //{
                //    p.createdAt = DateTime.Now;
                //});
                await context.SuccessfullTransasction.AddRangeAsync(successfullTransaction);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task TransactionNotFoundLib(List<TransactionNotFoundAtLIB> transactionNotFoundAtLIB, TrasactionReversalDbContext context)
        {
            try
            {
                //transactionNotFoundAtLIB.ForEach(p =>
                //{
                //    p.cretedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                //    p.CreatedDate = DateTime.Now;
                //});
                await _context.TransactionNotFoundAtLIB.AddRangeAsync(transactionNotFoundAtLIB);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SuccessfullTransaction>> GetSuccessfullTransaction(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);

                if (param.Date == new DateTime())
                {
                    param.Date = DateTime.Today;
                }
                return (await _context.SuccessfullTransasction.Where(p =>
                (p.TransactionType == param.TransactionType) &&
                (p.TransactionDate.Date == param.Date.Date) &&
                (param.AccountNo == null || p.DebitedAccountNumber == param.AccountNo) &&
                (param.GlAccountNo == null || p.CreditedAccount == param.GlAccountNo)).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionNotFoundAtEthSwitch>> GetTransactionNotFoundEthswitch(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                if (param.Date == new DateTime())
                {
                    param.Date = DateTime.Today;
                }
                return (await _context.TransactionNotFoundAtEthSwitch.Where(p =>
                (p.TransactionType == param.TransactionType) &&
                (p.TransactionDate.Date == param.Date.Date) &&
                (param.AccountNo == null || p.DebitedAccountNumber == param.AccountNo) &&
                (param.GlAccountNo == null || p.CreditedAccount == param.GlAccountNo)).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionNotFoundAtLIB>> GetTransactionNotFoundLib(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                if (param.Date == new DateTime())
                    param.Date = DateTime.Today;
                return (await _context.TransactionNotFoundAtLIB.Where(p =>
                (p.TransactionType == param.TransactionType) &&
                (p.TransactionDate.Date == param.Date.Date) &&
                (param.AccountNo == null || p.DebitedAccountNumber == param.AccountNo) &&
                (param.GlAccountNo == null || p.CreditedAccount == param.GlAccountNo)).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LibOutgoingTransaction>> GetLibOutgoingTransactionImports(SearchParams param)
        {
            try
            {
                if (param.DateTo != new DateTime())
                    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                //return await _context.LibOutgoingTransaction.Where(p =>
                return await _context.LibOutgoingTransaction.Where(p =>
                 p.CreatedAt >= param.DateFrom && p.CreatedAt <= param.DateTo).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LibIncommingTransaction>> GetLibIncommingTransactionImports(SearchParams param)
        {
            try
            {
                if (param.DateTo != new DateTime())
                    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                //return await _context.LibOutgoingTransaction.Where(p =>
                return await _context.LibIncommingTransaction.Where(p =>
                 p.TimeStamp >= param.DateFrom && p.TimeStamp <= param.DateTo).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CBSEthswichOutgoingTransaction>> GetCbsOutgoingTransactionImports(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                return await _context.CBSEthswichOutgoingTransaction.Where(p =>
                 p.date_1.Date == param.Date.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CBSEthswichIncomingTransaction>> GetCbsIncomingTransactionImports(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                return await _context.CBSEthswichIncomingTransaction.Where(p =>
                 p.date_1.Date == param.Date.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EthswitchOutgoingTransactionImport>> GetEthswichOutgoingTransactionImports(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                return await _context.EthswitchOutgoingTransactionImport
                    .Where(p => p.Transaction_Date.Date == param.Date.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EthswitchIncommingTransactionImport>> GetEthswichIncomingTransactionImports(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                return await _context.EthswitchIncommingTransactionImport
                    .Where(p => p.Transaction_Date.Date == param.Date.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Response> CheckReconsiledTransactionFound(SearchParams param)
        {
            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);

                var succesfulreconsilation = await _context.SuccessfullTransasction.AnyAsync(p =>
                 p.TransactionDate.Date == param.Date.Date);

                var notfoundLIBreconsilation = await _context.TransactionNotFoundAtLIB.AnyAsync(p =>
                 p.TransactionDate.Date == param.Date.Date);

                var notfoundEthreconsilation = await _context.TransactionNotFoundAtEthSwitch.AnyAsync(p =>
                 p.TransactionDate.Date == param.Date.Date);
                if (succesfulreconsilation || notfoundLIBreconsilation || notfoundEthreconsilation)
                {
                    return new Response()
                    {
                        message = "Reconsillation Already made for selected Date",
                        status = "-1"
                    };
                }

                return new Response()
                {
                    message = "",
                    status = "0"
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Response> CheckDataForReconsilation(SearchParams param)
        {
            try
            {

                var CbsIncoming = await _context.CBSEthswichIncomingTransaction.AnyAsync(p =>
                 p.date_1.Date == param.Date.Date);
                if (!CbsIncoming)
                {
                    await _iCbsTransactionImportRepo.ImportCbsIncomingTransaction(param.Date);
                }

                var CbsOutgoing = await _context.CBSEthswichOutgoingTransaction.AnyAsync(p =>
                p.date_1.Date == param.Date.Date);
                if (!CbsOutgoing)
                {
                    await _iCbsTransactionImportRepo.ImportCbsOutgoingTransaction(param.Date);
                }

                var EthIncomingCount = _context.EthswitchIncommingTransactionImport.Count(p =>
                p.Transaction_Date.Date == param.Date.Date);
                if (EthIncomingCount == 0)
                {
                    return new Response()
                    {
                        message = $"Please Import Ethiswich transaction file for {param.Date.Date}",
                        status = "-1"
                    };
                }

                var EthOutgoingCount = _context.EthswitchOutgoingTransactionImport.Count(p =>
                 p.Transaction_Date.Date == param.Date.Date);
                if (EthOutgoingCount == 0)
                {
                    return new Response()
                    {
                        message = $"Please Import Ethiswich transaction file for {param.Date.Date}",
                        status = "-1"
                    };
                }

                var CbsIncomingCount = _context.CBSEthswichIncomingTransaction.Count(p =>
                p.date_1.Date == param.Date.Date);

                var CbsOutgoingCount = _context.CBSEthswichOutgoingTransaction.Count(p =>
                p.date_1.Date == param.Date.Date);

                if (CbsIncomingCount == 0 || CbsOutgoingCount == 0 || EthIncomingCount == 0 || EthOutgoingCount == 0)
                {
                    return new Response()
                    {
                        message = "There is no enough Data to Made Reconsillation",
                        status = "-1"
                    };
                }
                else
                {
                    if (Math.Abs(CbsIncomingCount - EthIncomingCount) > 250)
                    {
                        return new Response()
                        {
                            message = "There is no enough Data to Made Reconsillation. please contact Administrator",
                            status = "-1"
                        };
                    }

                    if (Math.Abs(CbsOutgoingCount - (EthOutgoingCount * 3)) > 250)
                    {
                        return new Response()
                        {
                            message = "There is no enough Data to Made Reconsillation. please contact Administrator",
                            status = "-1"
                        };
                    }
                }

                return new Response()
                {
                    message = "",
                    status = "0"
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ReconsillationSummaryReportDto>> ReconsillationSummaryReport(SearchParams param)
        {

            List<ReconsillationSummaryReportDto> reconsillationSummaryReports = new List<ReconsillationSummaryReportDto>();

            try
            {
                //if (param.DateTo != new DateTime())
                //    param.DateTo = new DateTime(param.DateTo.Year, param.DateTo.Month, param.DateTo.Day, 23, 59, 59);
                ReconsillationSummaryReportDto objReconsillationSummaryReport = new ReconsillationSummaryReportDto();
                var successullList = await _context.SuccessfullTransasction
                    .Where(p => p.TransactionDate.Date >= param.DateFrom.Date && p.TransactionDate.Date <= param.DateTo.Date)
                    .ToListAsync();

                var PendingOnLiblList = await _context.TransactionNotFoundAtLIB
                    .Where(p => p.TransactionDate.Date >= param.DateFrom.Date && p.TransactionDate.Date <= param.DateTo.Date)
                    .ToListAsync();

                var PendingOnEthSwichlList = await _context.TransactionNotFoundAtEthSwitch
                .Where(p => p.TransactionDate.Date >= param.DateFrom.Date && p.TransactionDate.Date <= param.DateTo.Date)
                .ToListAsync();

                var dateList = successullList.Select(p => p.TransactionDate.Date).Distinct();

                foreach (var item in dateList)
                {
                    objReconsillationSummaryReport = new ReconsillationSummaryReportDto();
                    int IncommingsucCount = successullList.Count(p => p.TransactionDate.Date == item && p.TransactionType == "1");
                    int IncommingPendingLibCount = PendingOnLiblList.Count(p => p.TransactionDate.Date == item && p.TransactionType == "1");
                    int IncommingPendingEthCount = PendingOnEthSwichlList.Count(p => p.TransactionDate.Date == item && p.TransactionType == "1");
                    objReconsillationSummaryReport.TransactionDate = item.ToString();
                    objReconsillationSummaryReport.TransactionType = "Incomming Transaction";
                    objReconsillationSummaryReport.SuccessfullyReconsiledCount = IncommingsucCount;
                    objReconsillationSummaryReport.PendingOnLIBCount = IncommingPendingLibCount;
                    objReconsillationSummaryReport.PendingOnEthswichCount = IncommingPendingEthCount;
                    reconsillationSummaryReports.Add(objReconsillationSummaryReport);

                    objReconsillationSummaryReport = new ReconsillationSummaryReportDto();
                    int OutGoingsucCount = successullList.Count(p => p.TransactionDate.Date == item && p.TransactionType == "0");
                    int OutGoingPendingLibCount = PendingOnLiblList.Count(p => p.TransactionDate.Date == item && p.TransactionType == "0");
                    int OutGoingPendingEthCount = PendingOnEthSwichlList.Count(p => p.TransactionDate.Date == item && p.TransactionType == "0");
                    objReconsillationSummaryReport.TransactionDate = item.ToString();
                    objReconsillationSummaryReport.TransactionType = "Outgoing Transaction";
                    objReconsillationSummaryReport.SuccessfullyReconsiledCount = OutGoingsucCount;
                    objReconsillationSummaryReport.PendingOnLIBCount = OutGoingPendingLibCount;
                    objReconsillationSummaryReport.PendingOnEthswichCount = OutGoingPendingEthCount;
                    reconsillationSummaryReports.Add(objReconsillationSummaryReport);
                }
                return reconsillationSummaryReports.OrderBy(p=>p.TransactionDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
