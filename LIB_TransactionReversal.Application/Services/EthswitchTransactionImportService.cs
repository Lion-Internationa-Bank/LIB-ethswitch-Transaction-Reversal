using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Azure;
using LIB_Documentmanagement.DAL.Entity;
using LIB_Documentmanagement.DAL.Interface;
using LIB_TransactionReversal.Application.Interfaces;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.DTO;
using Newtonsoft.Json;

namespace LIB_TransactionReversal.Application.Services
{
    public class EthswitchTransactionImportService : IEthswitchTransactionImportService
    {
        private readonly IEthswitchTransactionImportRepository _ethswitchTransactionImportRepository;
        private readonly ITransactionReversalRepository _transactionReversalRepo;
        private readonly ITransactionAdjustementRepository _transactionAdjustementRepo;
        private readonly IMapper _mapper;
        private readonly TrasactionReversalDbContext _context;
        private readonly ICbsTransactionImportRepository _ICbsTransactionImportRepo;
        public EthswitchTransactionImportService(IEthswitchTransactionImportRepository ethswitchTransactionImportRepository,
            ITransactionReversalRepository transactionReversalRepo, IMapper mapper,
            TrasactionReversalDbContext context, ITransactionAdjustementRepository transactionAdjustementRepo,
            ICbsTransactionImportRepository iCbsTransactionImportRepo)
        {
            _ethswitchTransactionImportRepository = ethswitchTransactionImportRepository;
            _transactionReversalRepo = transactionReversalRepo;
            _mapper = mapper;
            _context= context;
            _transactionAdjustementRepo = transactionAdjustementRepo;
            _ICbsTransactionImportRepo = iCbsTransactionImportRepo;
        }

       

        public async Task<List<ImportEthswichTransactionDto>> ImportEthswitchTransaction(List<ImportEthswichTransactionDto> ethswitchTransactionImport)
        {
            try
            {
                var existinTransactionList = await _ethswitchTransactionImportRepository.CheckTransactionExist(ethswitchTransactionImport);
                if (existinTransactionList.Count > 0)
                {
                    foreach (var item in existinTransactionList)
                    {
                        item.IsAlreadyExist = true;
                    }
                    return existinTransactionList;
                }
                await _ethswitchTransactionImportRepository.ImportEthswitchTransaction(ethswitchTransactionImport);

                return ethswitchTransactionImport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<ImportEthswichTransactionDto>> ReconsilePendingTransaction(SearchParams searchParams)
        {
            try
            {
                // Check if Already reconsiled for selected Date
                var CheckReconsiled = await _ethswitchTransactionImportRepository.CheckReconsiledTransactionFound(searchParams);
                if(CheckReconsiled.status == "-1")
                {
                    throw new Exception((string)CheckReconsiled.message);
                }

                // Check if there is data for reconsillation
                var CheckReconsilationData = await _ethswitchTransactionImportRepository.CheckDataForReconsilation(searchParams);
                if (CheckReconsilationData.status == "-1")
                {
                    throw new Exception((string)CheckReconsilationData.message);
                }
                // Reconsile Incoming Transaction
                await ReconsileIncomingPendingTransaction(searchParams);

                // Reconsile Outgoing Transaction
                List<EthswitchOutgoingTransactionImport> ethtrans = await _ethswitchTransactionImportRepository.GetEthswichOutgoingTransactionImports(searchParams);
                
                List<DateTime> transDate = ethtrans.Select(p => p.Transaction_Date.Date).ToList();
                List<string> referenceNo = ethtrans.Select(p => p.Refnum_F37).ToList();
                List<CBSEthswichOutgoingTransaction> libcbstrans = await _ethswitchTransactionImportRepository.GetCbsOutgoingTransactionImports(searchParams);
                if(ethtrans.Count ==0 || libcbstrans.Count == 0)
                {
                    throw new Exception("No transaction found for the selected date");
                }
                var unsuccessfulTrans = libcbstrans.Where(x => x.status == "IG").ToList();
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        #region removed
                        //if (unsuccessfulTrans.Count > 0)
                        //{
                        //    var FindAtEthSwich = unsuccessfulTrans.Where(x => transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no)).ToList();
                        //    FindAtEthSwich.ForEach(tras =>
                        //    {
                        //        UpdateTransactionStatus updateTransaction = new UpdateTransactionStatus();
                        //        updateTransaction.acctno = tras.account_debited;
                        //        updateTransaction.transid = tras.ref_no;
                        //        string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                        //        string URL = "http://10.1.10.90:7000/api/lib/v1/updateTransactionStatus";
                        //        // string URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/checktransactionposibility";
                        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                        //        request.Method = "POST";
                        //        request.ContentType = "application/json";
                        //        request.ContentLength = updateTransactionData.Length;
                        //        using (Stream updatewebStream = request.GetRequestStream())
                        //        using (StreamWriter requestWriter = new StreamWriter(updatewebStream, System.Text.Encoding.ASCII))
                        //        {
                        //            requestWriter.Write(updateTransactionData);
                        //        }
                        //        WebResponse webResponse = request.GetResponse();
                        //        webResponse = request.GetResponse(); using (Stream updatewebStream = webResponse.GetResponseStream() ?? Stream.Null)
                        //        using (StreamReader updateresponseReader = new StreamReader(updatewebStream))
                        //        {
                        //            string updateresponse = updateresponseReader.ReadToEnd();
                        //        }


                        //    });
                        //    // await _ethswitchTransactionImportRepository.SuccessfullTransaction(_mapper.Map<List<SuccessfullTransaction>>(FindAtEthSwich));
                        //    var NotFindAtEthSwich = unsuccessfulTrans.Where(x => !(transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no))).ToList();

                        //    List<TransactionReversal> checkedTransactions = new List<TransactionReversal>();

                        //    NotFindAtEthSwich.ForEach(p =>
                        //    {
                        //        var transactionReversal = new TransactionReversal()
                        //        {
                        //            DebitedAccountNumber = p.account_debited,
                        //            Amount = p.amount,
                        //            branch = p.branch,
                        //            createdAt = p.date_1,
                        //            CreditedAccount = p.account_credited,
                        //            RefNo = p.ref_no,
                        //            Status = ""
                        //        };

                        //        if (transactionReversal.Amount >= 0 && transactionReversal.Amount <= 5000)
                        //        {
                        //            transactionReversal.ServiceFee = Convert.ToDecimal(0.004) * transactionReversal.Amount;
                        //            transactionReversal.VAT = Convert.ToDecimal(0.15) * transactionReversal.ServiceFee;
                        //            transactionReversal.TotalAmount = transactionReversal.Amount + transactionReversal.ServiceFee + transactionReversal.VAT;

                        //        }
                        //        if (transactionReversal.Amount > 5000)
                        //        {
                        //            transactionReversal.ServiceFee = Convert.ToDecimal(0.0024) * transactionReversal.Amount;
                        //            transactionReversal.VAT = Convert.ToDecimal(0.15) * transactionReversal.ServiceFee;
                        //            transactionReversal.TotalAmount = transactionReversal.Amount + transactionReversal.ServiceFee + transactionReversal.VAT;
                        //        }
                        //        //transactionReversal.Status = "Approved";
                        //        transactionReversal.Status = "Pending";

                        //        checkedTransactions.Add(transactionReversal);

                        //    });
                        //    await _transactionReversalRepo.SaveTransactionReversal(checkedTransactions, _context);

                        //}

                        #endregion

                        var successfulTrans = libcbstrans.Where(x => x.status == "VA" || x.status == "VF").ToList();

                        if (successfulTrans.Count > 0)
                        {
                            // find list of outgoing transaction that are found in cbs core and not in the ethswich that must be reversed

                            var NotFindAtEthSwich = successfulTrans.Where(x => !(transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no.Trim()))).ToList();
                            if (NotFindAtEthSwich.Count > 0)
                            {
                                searchParams.DateFrom = NotFindAtEthSwich.Min(p => p.date_1).Date;
                                searchParams.DateTo = NotFindAtEthSwich.Max(p => p.date_1).Date;

                                List<LibOutgoingTransaction> libtransforbankname = await _ethswitchTransactionImportRepository.GetLibOutgoingTransactionImports(searchParams);
                                NotFindAtEthSwich.ForEach(trans =>
                                {
                                    trans.date_1 = Convert.ToDateTime(string.Concat(trans.date_1.ToShortDateString(), " ", trans.time_1));
                                    trans.BankName = libtransforbankname.Where(p => p.Rrn == trans.ref_no).FirstOrDefault()?.BankName;
                                });
                                //save to db
                                await _ethswitchTransactionImportRepository.TransactionNotFoundEthswitch(_mapper.Map<List<TransactionNotFoundAtEthSwitch>>(NotFindAtEthSwich), _context);
                            }


                            List<TransactionReversal>  checkedTransactions = new List<TransactionReversal>();

                            NotFindAtEthSwich.ForEach(p =>
                            {
                                var transactionReversal = new TransactionReversal()
                                {
                                    DebitedAccountNumber = p.account_debited,
                                    Amount = p.amount,
                                    branch = p.branch,
                                    createdAt = p.date_1,
                                    CreditedAccount = p.account_credited,
                                    RefNo = p.ref_no,
                                    Status = "",
                                    
                                };

                                //if (transactionReversal.Amount >= 0 && transactionReversal.Amount <= 5000)
                                //{
                                //    transactionReversal.ServiceFee = Convert.ToDecimal(0.004) * transactionReversal.Amount;
                                //    transactionReversal.VAT = Convert.ToDecimal(0.15) * transactionReversal.ServiceFee;
                                //    transactionReversal.TotalAmount = transactionReversal.Amount + transactionReversal.ServiceFee + transactionReversal.VAT;

                                //}
                                //if (transactionReversal.Amount > 5000)
                                //{
                                //    transactionReversal.ServiceFee = Convert.ToDecimal(0.0024) * transactionReversal.Amount;
                                //    transactionReversal.VAT = Convert.ToDecimal(0.15) * transactionReversal.ServiceFee;
                                //    transactionReversal.TotalAmount = transactionReversal.Amount + transactionReversal.ServiceFee + transactionReversal.VAT;
                                //}
                                transactionReversal.Status = "Pending";

                                checkedTransactions.Add(transactionReversal);

                            });
                            //save trancsion lists that are for reversal
                            await _transactionReversalRepo.SaveTransactionReversal(checkedTransactions, _context);

                            //List of transaction that are in both
                            var FindInBoth = successfulTrans.Where(x => transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no.Trim())).ToList();

                            var FindInBothTransactions = _mapper.Map<List<SuccessfullTransaction>>(FindInBoth);
                            FindInBothTransactions.ForEach(tran =>
                            {
                                tran.LibTransactionDate = Convert.ToDateTime(string.Concat(tran.TransactionDate.ToShortDateString(),
                                    " ", (FindInBoth.Where(p=>p.ref_no == tran.RefNo).FirstOrDefault().time_1.ToString())));
                                tran.EthTransactionDate = ethtrans.Where(p => p.Refnum_F37 == tran.RefNo).FirstOrDefault()?.Transaction_Date;
                                tran.BankName = ethtrans.Where(p => p.Refnum_F37 == tran.RefNo).FirstOrDefault()?.Acquirer;
                            });
                            //save to database
                            await _ethswitchTransactionImportRepository.SuccessfullTransaction(FindInBothTransactions, _context);
                        }

                        List<DateTime> libdates = libcbstrans.Select(p => p.date_1).ToList();
                        List<string> libReferenceNo = libcbstrans.Select(p => p.ref_no.Trim()).ToList();

                        // list of transaction that are found in Ethswich and not on cbs core db
                        var notFoundLibSide = ethtrans.Where(x => !(libReferenceNo.Contains(x.Refnum_F37) && libdates.Contains(x.Transaction_Date.Date))).ToList();
                        if (notFoundLibSide.Count > 0)
                        {
                            searchParams.DateFrom = notFoundLibSide.Min(p => p.Transaction_Date).Date;
                            searchParams.DateTo = notFoundLibSide.Max(p => p.Transaction_Date).Date;
                            List<LibOutgoingTransaction> libtrans = await _ethswitchTransactionImportRepository.GetLibOutgoingTransactionImports(searchParams);
                            List<string> ethtrsNo = notFoundLibSide.Select(p => p.Refnum_F37).ToList();
                            var libtransnotinEth = libtrans.Where(p => ethtrsNo.Contains(p.Rrn)).ToList();
                            var TransactionNotFoundLIB = _mapper.Map<List<TransactionNotFoundAtLIB>>(libtransnotinEth);
                            TransactionNotFoundLIB.ForEach(trans =>
                            {
                                trans.EthTransactionDate = notFoundLibSide.Where(p => p.Refnum_F37 == trans.RefNo).FirstOrDefault().Transaction_Date;
                            });
                            // save to DB
                            await _ethswitchTransactionImportRepository.TransactionNotFoundLib(TransactionNotFoundLIB, _context);
                        }
                        //}
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ImportEthswichTransactionDto>> ReconsileIncomingPendingTransaction(SearchParams searchParams)
        {
            try
            {

                List<EthswitchIncommingTransactionImport> ethtrans = await _ethswitchTransactionImportRepository.GetEthswichIncomingTransactionImports(searchParams);

                List<DateTime> transDate = ethtrans.Select(p => p.Transaction_Date.Date).ToList();
                List<string> referenceNo = ethtrans.Select(p => p.Refnum_F37).ToList();
                List<CBSEthswichIncomingTransaction> libcbstrans = await _ethswitchTransactionImportRepository.GetCbsIncomingTransactionImports(searchParams);
                if (ethtrans.Count == 0 || libcbstrans.Count == 0)
                {
                    throw new Exception("No transaction found for the selected date");
                }
                var unsuccessfulTrans = libcbstrans.Where(x => x.status == "IG").ToList();
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        #region removed
                        //if (unsuccessfulTrans.Count > 0)
                        //{
                        //    var FindAtEthSwich = unsuccessfulTrans.Where(x => transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no)).ToList();
                        //    FindAtEthSwich.ForEach(tras =>
                        //    {
                        //        UpdateTransactionStatus updateTransaction = new UpdateTransactionStatus();
                        //        updateTransaction.acctno = tras.account_debited;
                        //        updateTransaction.transid = tras.ref_no;
                        //        string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                        //        string URL = "http://10.1.10.90:7000/api/lib/v1/updateTransactionStatus";
                        //        // string URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/checktransactionposibility";
                        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                        //        request.Method = "POST";
                        //        request.ContentType = "application/json";
                        //        request.ContentLength = updateTransactionData.Length;
                        //        using (Stream updatewebStream = request.GetRequestStream())
                        //        using (StreamWriter requestWriter = new StreamWriter(updatewebStream, System.Text.Encoding.ASCII))
                        //        {
                        //            requestWriter.Write(updateTransactionData);
                        //        }
                        //        WebResponse webResponse = request.GetResponse();
                        //        webResponse = request.GetResponse(); using (Stream updatewebStream = webResponse.GetResponseStream() ?? Stream.Null)
                        //        using (StreamReader updateresponseReader = new StreamReader(updatewebStream))
                        //        {
                        //            string updateresponse = updateresponseReader.ReadToEnd();
                        //        }


                        //    });
                        //    // await _ethswitchTransactionImportRepository.SuccessfullTransaction(_mapper.Map<List<SuccessfullTransaction>>(FindAtEthSwich));
                        //    var NotFindAtEthSwich = unsuccessfulTrans.Where(x => !(transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no))).ToList();

                        //    List<TransactionReversal> checkedTransactions = new List<TransactionReversal>();

                        //    NotFindAtEthSwich.ForEach(p =>
                        //    {
                        //        var transactionReversal = new TransactionReversal()
                        //        {
                        //            DebitedAccountNumber = p.account_debited,
                        //            Amount = p.amount,
                        //            branch = p.branch,
                        //            createdAt = p.date_1,
                        //            CreditedAccount = p.account_credited,
                        //            RefNo = p.ref_no,
                        //            Status = ""
                        //        };

                        //        if (transactionReversal.Amount >= 0 && transactionReversal.Amount <= 5000)
                        //        {
                        //            transactionReversal.ServiceFee = Convert.ToDecimal(0.004) * transactionReversal.Amount;
                        //            transactionReversal.VAT = Convert.ToDecimal(0.15) * transactionReversal.ServiceFee;
                        //            transactionReversal.TotalAmount = transactionReversal.Amount + transactionReversal.ServiceFee + transactionReversal.VAT;

                        //        }
                        //        if (transactionReversal.Amount > 5000)
                        //        {
                        //            transactionReversal.ServiceFee = Convert.ToDecimal(0.0024) * transactionReversal.Amount;
                        //            transactionReversal.VAT = Convert.ToDecimal(0.15) * transactionReversal.ServiceFee;
                        //            transactionReversal.TotalAmount = transactionReversal.Amount + transactionReversal.ServiceFee + transactionReversal.VAT;
                        //        }
                        //        //transactionReversal.Status = "Approved";
                        //        transactionReversal.Status = "Pending";

                        //        checkedTransactions.Add(transactionReversal);

                        //    });
                        //    await _transactionReversalRepo.SaveTransactionReversal(checkedTransactions, _context);

                        //}

                        #endregion

                        var successfulTrans = libcbstrans.Where(x => x.status != "IG").ToList();

                        if (successfulTrans.Count > 0)
                        {
                            //get transactionlist find on cbs but not in Ethiswich
                            var NotFindAtEthSwich = successfulTrans.Where(x => !(transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no.Trim()))).ToList();
                            if (NotFindAtEthSwich.Count > 0)
                            {
                                searchParams.DateFrom = NotFindAtEthSwich.Min(p => p.date_1).Date;
                                searchParams.DateTo = NotFindAtEthSwich.Max(p => p.date_1).Date;

                                // check local database to find the bank name that doesnt found in cbs core db

                                List<LibIncommingTransaction> libtransforbankname = await _ethswitchTransactionImportRepository.GetLibIncommingTransactionImports(searchParams);
                                NotFindAtEthSwich.ForEach(trans =>
                                {
                                    trans.date_1 = Convert.ToDateTime(string.Concat(trans.date_1.ToShortDateString(), " ", trans.time_1));
                                    trans.BankName = libtransforbankname.Where(p => p.EthswitchRefNo == trans.ref_no).FirstOrDefault()?.BankName;
                                    trans.TransactionType = "1";
                                });

                                //save to mysql database
                                await _ethswitchTransactionImportRepository.TransactionNotFoundEthswitch(_mapper.Map<List<TransactionNotFoundAtEthSwitch>>(NotFindAtEthSwich), _context);
                            }
                            //get transactionlist find on both cbs and Ethiswich

                            var FindInBoth = successfulTrans.Where(x => transDate.Contains(x.date_1) && referenceNo.Contains(x.ref_no.Trim())).ToList();

                            var FindInBothTransactions = _mapper.Map<List<SuccessfullTransaction>>(FindInBoth);

                            //get ethswich transaction date and bank name that doent foun in cbs core
                            FindInBothTransactions.ForEach(tran =>
                            {
                                tran.LibTransactionDate = Convert.ToDateTime(string.Concat(tran.TransactionDate.ToShortDateString(),
                                    " ", (FindInBoth.Where(p => p.ref_no == tran.RefNo).FirstOrDefault().time_1.ToString())));
                                tran.EthTransactionDate = ethtrans.Where(p => p.Refnum_F37 == tran.RefNo).FirstOrDefault()?.Transaction_Date;
                                tran.BankName = ethtrans.Where(p => p.Refnum_F37 == tran.RefNo).FirstOrDefault()?.Issuer;
                                tran.TransactionType = "1";

                            });

                            // save to db
                            await _ethswitchTransactionImportRepository.SuccessfullTransaction(FindInBothTransactions, _context);
                        }


                        List<DateTime> libdates = libcbstrans.Select(p => p.date_1).ToList();
                        List<string> libReferenceNo = libcbstrans.Select(p => p.ref_no.Trim()).ToList();

                        //get transactionlist find  on Ethswich but not in cbs thous records are for Adjustement 

                        var notFoundLibSide = ethtrans.Where(x => !(libReferenceNo.Contains(x.Refnum_F37) && libdates.Contains(x.Transaction_Date.Date))).ToList();
                        if (notFoundLibSide.Count > 0)
                        {
                            searchParams.DateFrom = notFoundLibSide.Min(p => p.Transaction_Date).Date;
                            searchParams.DateTo = notFoundLibSide.Max(p => p.Transaction_Date).Date;
                            List<LibIncommingTransaction> libtrans = await _ethswitchTransactionImportRepository.GetLibIncommingTransactionImports(searchParams);
                            List<string> ethtrsNo = notFoundLibSide.Select(p => p.Refnum_F37).ToList();
                            var libtransnotinEth = libtrans.Where(p => ethtrsNo.Contains(p.EthswitchRefNo)).ToList();
                            var TransactionNotFoundLIB = new List<TransactionNotFoundAtLIB>();
                            List<TransactionAdjustement> findforPaymentTransactions = new List<TransactionAdjustement>();

                            if (libtransnotinEth.Count > 0)
                            {
                                List<string> accounts = libtransnotinEth.Select(p => p.Account).ToList();

                                // get account branch from core database for each record for adjustement
                                var accountList = await _ICbsTransactionImportRepo.GetAccountBranch(accounts);

                                TransactionNotFoundLIB = _mapper.Map<List<TransactionNotFoundAtLIB>>(libtransnotinEth);
                                TransactionNotFoundLIB.ForEach(trans =>
                                {
                                    trans.EthTransactionDate = notFoundLibSide.Where(p => p.Refnum_F37 == trans.RefNo).FirstOrDefault()?.Transaction_Date;
                                    trans.Branch = accountList.Where(p => p.accountnumber == trans.DebitedAccountNumber).FirstOrDefault()?.branch;
                                    trans.CustomerName = accountList.Where(p => p.accountnumber == trans.DebitedAccountNumber).FirstOrDefault()?.FULLNAME;
                                    trans.TransactionType = "1";
                                });

                                var SelectedForAdustement = TransactionNotFoundLIB;
                                SelectedForAdustement.ForEach(p =>
                                {
                                    var transactionAdjustement = new TransactionAdjustement()
                                    {
                                        CreditedAccount = p.DebitedAccountNumber,
                                        Amount = p.Amount,
                                        branch = p.Branch,
                                        CustomerName = p.CustomerName,
                                        createdAt = p.TransactionDate,
                                        RefNo = p.RefNo,
                                        Status = "Pending",

                                    };
                                    findforPaymentTransactions.Add(transactionAdjustement);

                                });
                            }

                            // canot find on lib core and local DB
                            var librefNums = libtrans.Select(p => p.EthswitchRefNo);
                            var tranNotfindLocalDB = notFoundLibSide.Where(p => !librefNums.Contains(p.Refnum_F37)).ToList();

                            tranNotfindLocalDB.ForEach(item =>
                            {
                                var NotFoundAtLIBDB = new TransactionNotFoundAtLIB()
                                {
                                    Amount = item.Amount,
                                    BankName = item.Issuer,
                                    RefNo = item.Refnum_F37,
                                    EthTransactionDate = item.Transaction_Date,
                                    TransactionDate = item.Transaction_Date,
                                    TransactionType = "1",
                                    Reason = "Transaction not found on LIB Core and Local DB",
                                };
                                TransactionNotFoundLIB.Add(NotFoundAtLIBDB);

                                var transactionAdjustement = new TransactionAdjustement()
                                {
                                    Amount = item.Amount,
                                    createdAt = item.Transaction_Date,
                                    RefNo = item.Refnum_F37,
                                    Status = "Pending",

                                };
                                findforPaymentTransactions.Add(transactionAdjustement);
                            });


                            await _transactionAdjustementRepo.SaveTransactionAdjustement(findforPaymentTransactions, _context);

                            await _ethswitchTransactionImportRepository.TransactionNotFoundLib(TransactionNotFoundLIB, _context);






                        }

                        ///////
                        //}
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }

                return null;
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
                return await _ethswitchTransactionImportRepository.GetSuccessfullTransaction(param);
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
                return await _ethswitchTransactionImportRepository.GetTransactionNotFoundEthswitch(param);
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
                return await _ethswitchTransactionImportRepository.CheckTransactionRangeExist(from, to);
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
                return await _ethswitchTransactionImportRepository.GetTransactionNotFoundLib(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        public async Task<List<ImportEthswichTransactionDto>> GetImportedTransaction(SearchParams param)
        {
            try
            {
                List<ImportEthswichTransactionDto> impotedTransaction = new List<ImportEthswichTransactionDto>();
                if(param.TransactionType=="0" || string.IsNullOrEmpty(param.TransactionType) || param.TransactionType == "null")
                {
                    List<EthswitchOutgoingTransactionImport> ethOuttrans = await _ethswitchTransactionImportRepository.GetEthswichOutgoingTransactionImports(param);
                    impotedTransaction.AddRange(_mapper.Map<List<ImportEthswichTransactionDto>>(ethOuttrans));
                }

                if (param.TransactionType == "1" || string.IsNullOrEmpty(param.TransactionType) || param.TransactionType == "null")
                {
                    List<EthswitchIncommingTransactionImport> ethInctrans = await _ethswitchTransactionImportRepository.GetEthswichIncomingTransactionImports(param);
                    impotedTransaction.AddRange(_mapper.Map<List<ImportEthswichTransactionDto>>(ethInctrans));
                }
                return impotedTransaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DAL.DTO.Response> CheckReconsiledTransactionFound(SearchParams param)
        {
            try
            {
                return await _ethswitchTransactionImportRepository.CheckReconsiledTransactionFound(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ReconsillationSummaryReportDto>> ReconsillationSummaryReport(SearchParams param)
        {
            try
            {
                return await _ethswitchTransactionImportRepository.ReconsillationSummaryReport(param);
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
                return await _ethswitchTransactionImportRepository.InsertInvalidDateEthiswichTransaction(ethswitchTransactionImport);
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
                return await _ethswitchTransactionImportRepository.GetInvalidEthiswichDateTransaction(param);
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
                return await _ICbsTransactionImportRepo.GetAccountDetail(accountNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    //public class libtransResponse
    //{
    //    public bool success { get; set; }
    //    public List<LIBTransaction> transactions { get; set; }
    //}

    //public class LIBTransaction
    //{
    //    public string _id { get; set; }
    //    public string accountNumber { get; set; }
    //    public string receiverAccount { get; set; }
    //    public decimal amount { get; set; }
    //    public string branch { get; set; }
    //    public string rrn { get; set; }
    //    public string statusEthswitch { get; set; }
    //    public string statusTransfer { get; set; }
    //    public DateTime createdAt { get; set; }
    //}

    public class UpdateTransactionStatus
    {
        public string acctno { get; set; }
        public string transid { get; set; }
    }
}
