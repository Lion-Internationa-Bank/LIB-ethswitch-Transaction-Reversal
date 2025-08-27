using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_Documentmanagement.DAL.Entity;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_Usermanagement.DAL.DTO;

namespace LIB_TransactionReversal.Application.Interfaces
{
    public interface IEthswitchTransactionImportService
    {
        Task<List<ImportEthswichTransactionDto>> ImportEthswitchTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport);
        Task<List<SuccessfullTransaction>> GetSuccessfullTransaction(SearchParams param);
        Task<List<TransactionNotFoundAtEthSwitch>> GetTransactionNotFoundEthswitch(SearchParams param);
         Task<List<TransactionNotFoundAtLIB>> GetTransactionNotFoundLib(SearchParams param);
        Task<List<ImportEthswichTransactionDto>> ReconsilePendingTransaction(SearchParams searchParams);

        Task<List<EthswitchOutgoingTransactionImport>> CheckTransactionRangeExist(DateTime from, DateTime to);

        Task<List<ImportEthswichTransactionDto>> GetImportedTransaction(SearchParams param);
        Task<Response> CheckReconsiledTransactionFound(SearchParams param);
        Task<List<ReconsillationSummaryReportDto>> ReconsillationSummaryReport(SearchParams param);

        Task<List<ImportEthswichTransactionDto>> InsertInvalidDateEthiswichTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport);
        Task<List<EthswitchInvalidDateTransaction>> GetInvalidEthiswichDateTransaction(SearchParams param);

        Task<AccountBranch> GetAccountDetail(string accountNo);

    }
}
