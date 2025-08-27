using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_Documentmanagement.DAL.Entity;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.DTO;

namespace LIB_TransactionReversal.DAL.Interface
{
    public interface IEthswitchTransactionImportRepository
    {
        Task ImportEthswitchTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport);
        Task TransactionNotFoundEthswitch(List<TransactionNotFoundAtEthSwitch> transactionNotFoundAtEthSwitch, TrasactionReversalDbContext context);
        Task SuccessfullTransaction(List<SuccessfullTransaction> successfullTransaction, TrasactionReversalDbContext context);
        Task TransactionNotFoundLib(List<TransactionNotFoundAtLIB> transactionNotFoundAtLIB, TrasactionReversalDbContext context);
        Task<List<SuccessfullTransaction>> GetSuccessfullTransaction(SearchParams param);
        Task<List<TransactionNotFoundAtEthSwitch>> GetTransactionNotFoundEthswitch(SearchParams param);
        Task<List<TransactionNotFoundAtLIB>> GetTransactionNotFoundLib(SearchParams param);
        Task<List<ImportEthswichTransactionDto>> CheckTransactionExist(List<ImportEthswichTransactionDto> ethswitchTransactionImport);
        Task<List<LibOutgoingTransaction>> GetLibOutgoingTransactionImports(SearchParams param);

        Task<List<LibIncommingTransaction>> GetLibIncommingTransactionImports(SearchParams param);

        Task<List<EthswitchOutgoingTransactionImport>> GetEthswichOutgoingTransactionImports(SearchParams param);
        Task<List<EthswitchIncommingTransactionImport>> GetEthswichIncomingTransactionImports(SearchParams param);
        Task<List<CBSEthswichIncomingTransaction>> GetCbsIncomingTransactionImports(SearchParams param);
        Task<List<EthswitchOutgoingTransactionImport>> CheckTransactionRangeExist(DateTime from, DateTime to);
        Task<List<ImportEthswichTransactionDto>> InsertInvalidDateEthiswichTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport);
        Task<List<EthswitchInvalidDateTransaction>> GetInvalidEthiswichDateTransaction(SearchParams param);

        Task<List<CBSEthswichOutgoingTransaction>> GetCbsOutgoingTransactionImports(SearchParams param);

        Task<Response> CheckReconsiledTransactionFound(SearchParams param);

        Task<Response> CheckDataForReconsilation(SearchParams param);

        Task<List<ReconsillationSummaryReportDto>> ReconsillationSummaryReport(SearchParams param);
    }
}
