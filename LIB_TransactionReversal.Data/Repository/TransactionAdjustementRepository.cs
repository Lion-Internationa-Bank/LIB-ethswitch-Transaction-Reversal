using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LIB_Documentmanagement.DAL.Entity;
using LIB_Documentmanagement.Infra.Data.Repository;
using LIB_Usermanagement.DAL.DTO;
using LIB_Usermanagement.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using LIB_TransactionReversal.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Interface;

namespace LIB_TransactionReversal.Infra.Data.Repository
{
    public class TransactionAdjustementRepository : ITransactionAdjustementRepository
    {
        private TrasactionReversalDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ICbsTransactionImportRepository _iCbsTransactionImportRepo;
        public TransactionAdjustementRepository(TrasactionReversalDbContext dbContext, IConfiguration configuration,
                                             IHttpContextAccessor httpContextAccessor, ICbsTransactionImportRepository iCbsTransactionImportRepo)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _iCbsTransactionImportRepo = iCbsTransactionImportRepo;
        }

        public async Task SaveTransactionAdjustement(TransactionAdjustement transactionAdjustement)
        {
            try
            {
                if (transactionAdjustement.Amount >= 0 && transactionAdjustement.Amount <= 5000)
                {
                    transactionAdjustement.ServiceFee = Convert.ToDecimal(0.004) * transactionAdjustement.Amount;
                    transactionAdjustement.VAT = Convert.ToDecimal(0.15) * transactionAdjustement.ServiceFee;
                    transactionAdjustement.TotalAmount = transactionAdjustement.Amount + transactionAdjustement.ServiceFee + transactionAdjustement.VAT;

                }
                if (transactionAdjustement.Amount > 5000)
                {
                    transactionAdjustement.ServiceFee = Convert.ToDecimal(0.0024) * transactionAdjustement.Amount;
                    transactionAdjustement.VAT = Convert.ToDecimal(0.15) * transactionAdjustement.ServiceFee;
                    transactionAdjustement.TotalAmount = transactionAdjustement.Amount + transactionAdjustement.ServiceFee + transactionAdjustement.VAT;
                }
                transactionAdjustement.Status = "Approved";
                transactionAdjustement.ApprovedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                transactionAdjustement.ApprovedDate = DateTime.Now;

                _dbContext.TransactionAdjustement.Add(transactionAdjustement);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteTransactionAdjustement(int Id)
        {
            try
            {
                var TransactionReversalDelete = _dbContext.TransactionAdjustement.SingleOrDefault(p => p.Id == Id);
                _dbContext.Remove(TransactionReversalDelete);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TransactionAdjustement> GetTransactionAdjustement(int Id)
        {
            try
            {
                return await _dbContext.TransactionAdjustement.SingleOrDefaultAsync(p => p.Id == Id); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionAdjustement>> GetTransactionAdjustement(SearchParams param)
        {
            try
            {
                if(param.Date == new DateTime())
                {
                    param.Date = DateTime.Today;
                }
                return await _dbContext.TransactionAdjustement.Where(p => p.Status != "Paid" &&
                (p.createdAt.Date == param.Date.Date) &&
                (param.AccountNo == null || p.CreditedAccount == param.AccountNo) &&
                (param.ReferenceNo == null || p.RefNo == param.ReferenceNo) &&
                ((param.Status == "0" && p.Status != "Pending") || p.Status == param.Status)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TransactionAdjustement>> GetAdjustementReport(SearchParams param)
        {
            try
            {
                return await _dbContext.TransactionAdjustement.Where(p => p.Status == "Paid" &&
                (param.DateFrom == new DateTime() || p.createdAt.Date >= param.DateFrom) &&
                (param.DateTo == new DateTime() || p.createdAt.Date <= param.DateTo) &&
                (param.AccountNo == null || p.CreditedAccount == param.AccountNo)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatTransactionAdjustement(TransactionAdjustement transactionAdjustement)
        {
            var updateddoc = _dbContext.TransactionAdjustement.Update(transactionAdjustement);
            await _dbContext.SaveChangesAsync();
        }

        public async Task updateTransactionAdjustementStatus(int id, string status, string message, string user = "", string transactionId = "")
        {
            try
            {
                var trasToUpdate = await _dbContext.TransactionAdjustement.SingleOrDefaultAsync(p => p.Id == id);
                trasToUpdate.Status = status;
                trasToUpdate.Message = message;
                if (user != "")
                {
                    trasToUpdate.PaidBy = user;
                    trasToUpdate.PaymentDate = DateTime.Now;
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveTransactionAdjustement(List<TransactionAdjustement> transactionAdjustementList, TrasactionReversalDbContext context)
        {
            try
            {
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
                //transactionReversal.Status = "Approved";
                //transactionReversal.ApprovedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                //transactionReversal.ApprovedDate = DateTime.Now;

                await context.TransactionAdjustement.AddRangeAsync(transactionAdjustementList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task CheckedPendingTransactionForReversal(List<int> transactionAdjustementIdList)
        {
            try
            {

                (await _dbContext.TransactionAdjustement.Where(p => transactionAdjustementIdList.Contains(p.Id)).ToListAsync()).ForEach(tran =>
                {
                    tran.Status = "Approved";
                    tran.ApprovedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                    tran.ApprovedDate = DateTime.Now;
                });
                await _dbContext.SaveChangesAsync();
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
                ServicePointManager.ServerCertificateValidationCallback =
                        (sender, certificate, chain, sslPolicyErrors) => true;

                var tras = await GetTransactionAdjustement(Id);
                Transaction transaction = new Transaction();
                transaction.amount = Convert.ToDecimal(tras.Amount);
                transaction.accountCredited = tras.CreditedAccount;
                transaction.branch = tras.branch;
                transaction.transactionId = tras.RefNo;
                transaction.Id = tras.Id;

                string DATA = JsonConvert.SerializeObject(transaction);

                //var checkposibility = await checktransactionposibility(transaction);
                //if (checkposibility.status != "valid")
                //{
                //    return checkposibility;
                //}
                string URL = "https://10.1.22.198:4040/createtransferfailedethiswitchtocustomer";
                //string URL = _configuration["EndPointUrl:CreateTransfere"] + "/createTransferreverse";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = DATA.Length;
                try
                {
                    using (Stream webStream = request.GetRequestStream())
                    using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                    {
                        requestWriter.Write(DATA);
                    }


                    WebResponse webResponse = request.GetResponse();
                    using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        Response res = JsonConvert.DeserializeObject<Response>(response);
                        if (res.status == "1")
                        {
                            string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                            await updateTransactionAdjustementStatus(tras.Id, "Paid", "", user, res.trnsid);

                            try
                            {
                                //UpdateTransaction updateTransaction = new UpdateTransaction();
                                //updateTransaction.acctno = tras.CreditedAccount;
                                //updateTransaction.transid = tras.RefNo;
                                //string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                                //URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/reverseTransactionStatus";
                                //request = (HttpWebRequest)WebRequest.Create(URL);
                                //request.Method = "POST";
                                //request.ContentType = "application/json";
                                //request.ContentLength = updateTransactionData.Length;

                            }
                            catch (Exception ex) { }
                            //return Ok(res);
                            return res;
                        }
                        else
                        {
                            var validate = await _iCbsTransactionImportRepo.ValidTransactionSuccess(transaction.transactionId);
                            if (validate.Count > 0)
                            {
                                string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                                await updateTransactionAdjustementStatus(tras.Id, "Paid", "", user, res.trnsid);
                            }
                            else
                            {
                                res.status = "-1";
                                res.message = "Transaction is not succed please try";
                            }
                           // await updateTransactionAdjustementStatus(tras.Id, "UnResponsive", "Payment Process UnResponsive");
                           //return Ok(res);
                            return res;
                        }

                    }
                }
                catch
                {
                    var validate = await _iCbsTransactionImportRepo.ValidTransactionSuccess(transaction.transactionId);
                    if (validate.Count > 0)
                    {
                        string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                        await updateTransactionAdjustementStatus(tras.Id, "Paid", "", user, transaction.transactionId);

                        return new Response()
                        {
                            status = "2",
                            message = "Adjudtement made Successfully"
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            status = "0",
                            message = "Transaction is not succed please try"
                        };
                    }
                   
                }
            }

            catch (Exception ex)
            {
                return new Response()
                {
                    status = "0",
                    message = "Unable to create reversal"
                };

            }
        }

        //public async Task<Response> checktransactionposibility(Transaction transaction)
        //{
        //    ServicePointManager.ServerCertificateValidationCallback =
        //        (sender, certificate, chain, sslPolicyErrors) => true;
        //    try
        //    {


        //        CheckStatus checkAcc = new CheckStatus();
        //        checkAcc.amtt = transaction.amount;
        //        checkAcc.account = transaction.accountCredited;
        //        string checkAccpay = JsonConvert.SerializeObject(checkAcc);
        //        //string URL = "http://10.1.10.90:7000/api/lib/v1/checktransactionposibility";
        //        string URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/checktransactionposibility";
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.ContentLength = checkAccpay.Length;

        //        using (Stream webStream = request.GetRequestStream())
        //        using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
        //        {
        //            requestWriter.Write(checkAccpay);
        //        }


        //        WebResponse webResponse = request.GetResponse();
        //        using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
        //        using (StreamReader responseReader = new StreamReader(webStream))
        //        {
        //            string response = responseReader.ReadToEnd();
        //            Response res = JsonConvert.DeserializeObject<Response>(response);
        //            if (res.status != "valid")
        //            {
        //                res.status = "-1";
        //                res.message = "Account Number is not Allowed to create transfere";
        //                await updateTransactionReversalStatus(transaction.Id, "UnResponsive", "Account Number is not Allowed to create transfere");
        //            }
        //            return res;



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task updateTransactionAccountNumber(UpdateTransactionAccountDto objTranAdjustement)
        {
            try
            {
                var trasToUpdate = await _dbContext.TransactionAdjustement.SingleOrDefaultAsync(p => p.Id == objTranAdjustement.Id && p.RefNo== objTranAdjustement.RefNo);
                trasToUpdate.CreditedAccount = objTranAdjustement.CreditedAccount;
                trasToUpdate.CustomerName = objTranAdjustement.CustomerName;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

