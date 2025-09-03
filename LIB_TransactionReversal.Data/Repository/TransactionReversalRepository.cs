using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using LIB_Documentmanagement.DAL.Entity;
using LIB_Documentmanagement.DAL.Interface;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.DTO;
using LIB_Usermanagement.DAL.Entity.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LIB_Documentmanagement.Infra.Data.Repository
{
    public class TransactionReversalRepository : ITransactionReversalRepository
    {
        private TrasactionReversalDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ICbsTransactionImportRepository _iCbsTransactionImportRepo;
        public TransactionReversalRepository(TrasactionReversalDbContext dbContext, IConfiguration configuration,
                                             IHttpContextAccessor httpContextAccessor,
                                             ICbsTransactionImportRepository iCbsTransactionImportRepo)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _iCbsTransactionImportRepo = iCbsTransactionImportRepo;
        }

        public async Task SaveTransactionReversal(TransactionReversal transactionReversal)
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

                //_dbContext.TransactionReversal.Add(transactionReversal);
                //await _dbContext.SaveChangesAsync();
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
                var TransactionReversalDelete = _dbContext.TransactionReversal.SingleOrDefault(p => p.Id == Id);
                _dbContext.Remove(TransactionReversalDelete);
                await _dbContext.SaveChangesAsync();
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
                return await _dbContext.TransactionReversal.SingleOrDefaultAsync(p => p.Id == Id); ;
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
                if (param.Date == new DateTime())
                    param.Date = DateTime.Today;
                return await _dbContext.TransactionReversal.Where(p => p.Status != "Reverse" &&
                (p.createdAt.Date == param.Date.Date) &&
                (param.AccountNo == null || p.DebitedAccountNumber == param.AccountNo) &&
                (param.ReferenceNo == null || p.RefNo == param.ReferenceNo) &&
                ((param.Status == "0" && p.Status != "Pending") || p.Status == param.Status)).ToListAsync();
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
                return await _dbContext.TransactionReversal.Where(p => p.Status == "Reverse" &&
                (param.DateFrom == new DateTime() || p.createdAt >= param.DateFrom) &&
                (param.DateTo == new DateTime() || p.createdAt <= param.DateTo) &&
                (param.AccountNo == null || p.DebitedAccountNumber == param.AccountNo)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatTransactionReversal(TransactionReversal transactionReversal)
        {
            var updateddoc = _dbContext.TransactionReversal.Update(transactionReversal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task updateTransactionReversalStatus(int id, string status, string message, string user = "", string transactionId = "")
        {
            try
            {
                var trasToUpdate = await _dbContext.TransactionReversal.SingleOrDefaultAsync(p => p.Id == id);
                trasToUpdate.Status = status;
                trasToUpdate.Message = message;
                if (user != "")
                {
                    trasToUpdate.ReversedBy = user;
                    trasToUpdate.ReversalDate = DateTime.Now;
                }
                if (transactionId != "")
                {
                    trasToUpdate.newTransaction = transactionId;
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveTransactionReversal(List<TransactionReversal> transactionReversalList, TrasactionReversalDbContext context)
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

                await context.TransactionReversal.AddRangeAsync(transactionReversalList);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task CheckedPendingTransactionForReversal(List<int> transactionReversalIdList)
        {
            try
            {

                (await _dbContext.TransactionReversal.Where(p => transactionReversalIdList.Contains(p.Id)).ToListAsync()).ForEach(tran =>
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

                var tras = await GetTransactionReversal(Id);
                if (tras.CreditedAccount == "11510305000")
                {
                    await CreatePrincipalReversal(tras);
                }
                else if (tras.CreditedAccount == "11510305001")
                {
                    await CreateServiceChargeReversal(tras);
                }
                else if (tras.CreditedAccount == "24101900001")
                {
                    await CreateVATReversal(tras);
                }

                //Transaction transaction = new Transaction();
                //transaction.amount = Convert.ToDecimal(tras.Amount);
                //transaction.accountCredited = tras.DebitedAccountNumber;
                //transaction.branch = tras.branch;
                //transaction.transactionId = tras.RefNo;
                //transaction.Id = tras.Id;
                //string DATA = JsonConvert.SerializeObject(transaction);

                ////var checkposibility = await checktransactionposibility(transaction);
                ////if (checkposibility.status != "valid")
                ////{
                ////    return checkposibility;
                ////}
                ////URL = "https://10.1.22.198:4040/createTransferreverse";
                //string URL = _configuration["EndPointUrl:CreateTransfere"] + "/createTransferreverseoutgoing";
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                //request.Method = "POST";
                //request.ContentType = "application/json";
                //request.ContentLength = DATA.Length;
                //try
                //{
                //    using (Stream webStream = request.GetRequestStream())
                //    using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                //    {
                //        requestWriter.Write(DATA);
                //    }


                //    WebResponse webResponse = request.GetResponse();
                //    using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                //    using (StreamReader responseReader = new StreamReader(webStream))
                //    {
                //        string response = responseReader.ReadToEnd();
                //        Response res = JsonConvert.DeserializeObject<Response>(response);
                //        if (res.status == "1")
                //        {
                //            string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                //            await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                //            try
                //            {
                //                UpdateTransaction updateTransaction = new UpdateTransaction();
                //                updateTransaction.acctno = tras.DebitedAccountNumber;
                //                updateTransaction.transid = tras.RefNo;
                //                string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                //                URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/reverseTransactionStatus";
                //                request = (HttpWebRequest)WebRequest.Create(URL);
                //                request.Method = "POST";
                //                request.ContentType = "application/json";
                //                request.ContentLength = updateTransactionData.Length;
                //                //using (Stream updatewebStream = request.GetRequestStream())
                //                //using (StreamWriter requestWriter = new StreamWriter(updatewebStream, System.Text.Encoding.ASCII))
                //                //{
                //                //    requestWriter.Write(updateTransactionData);
                //                //}
                //                //webResponse = request.GetResponse(); using (Stream updatewebStream = webResponse.GetResponseStream() ?? Stream.Null)
                //                //using (StreamReader updateresponseReader = new StreamReader(updatewebStream))
                //                //{
                //                //    string updateresponse = updateresponseReader.ReadToEnd();
                //                //}
                //            }
                //            catch (Exception ex) { }
                //            //return Ok(res);
                //            return res;
                //        }
                //        else
                //        {

                //            var validate = await _iCbsTransactionImportRepo.ValidTransactionSuccess(transaction.transactionId);
                //            if (validate.Count > 0)
                //            {
                //                string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                //                await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                //                res.status = "1";
                //                res.message = "Transaction reversa made successfully";
                //            }
                //            else
                //            {
                //                res.status = "-1";
                //                res.message = "Transaction is not succed please try";
                //            }
                //            //await updateTransactionReversalStatus(tras.Id, "UnResponsive", "Revesal Process UnResponsive");
                //            //return Ok(res);
                //            return res;
                //        }

                //    }
                //}
                //catch
                //{
                //    var validate = await _iCbsTransactionImportRepo.ValidTransactionSuccess(transaction.transactionId);
                //    if (validate.Count > 0)
                //    {
                //        string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                //        await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, transaction.transactionId);

                //        return new Response()
                //        {
                //            status = "1",
                //            message = "Transaction reversa made successfully"
                //        };
                //    }
                //    else
                //    {
                //        return new Response()
                //        {
                //            status = "-1",
                //            message = "Transaction reversal not succed please try"
                //        };
                //    }

                //}
                return new Response()
                {
                    status = "0",
                    message = "Unable to create reversal"
                };
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



        public async Task<Response> CreatePrincipalReversal(TransactionReversal tras)
        {
            Transaction transaction = new Transaction();
            transaction.amount = Convert.ToDecimal(tras.Amount);
            transaction.accountCredited = tras.DebitedAccountNumber;
            transaction.branch = tras.branch;
            transaction.transactionId = tras.RefNo;
            transaction.Id = tras.Id;
            string DATA = JsonConvert.SerializeObject(transaction);

            //var checkposibility = await checktransactionposibility(transaction);
            //if (checkposibility.status != "valid")
            //{
            //    return checkposibility;
            //}
            //URL = "https://10.1.22.198:4040/createTransferreverse";
            string URL = _configuration["EndPointUrl:createPrincipalTransferReverse"];
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
                        await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                        try
                        {
                            UpdateTransaction updateTransaction = new UpdateTransaction();
                            updateTransaction.acctno = tras.DebitedAccountNumber;
                            updateTransaction.transid = tras.RefNo;
                            string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                            URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/reverseTransactionStatus";
                            request = (HttpWebRequest)WebRequest.Create(URL);
                            request.Method = "POST";
                            request.ContentType = "application/json";
                            request.ContentLength = updateTransactionData.Length;
                            //using (Stream updatewebStream = request.GetRequestStream())
                            //using (StreamWriter requestWriter = new StreamWriter(updatewebStream, System.Text.Encoding.ASCII))
                            //{
                            //    requestWriter.Write(updateTransactionData);
                            //}
                            //webResponse = request.GetResponse(); using (Stream updatewebStream = webResponse.GetResponseStream() ?? Stream.Null)
                            //using (StreamReader updateresponseReader = new StreamReader(updatewebStream))
                            //{
                            //    string updateresponse = updateresponseReader.ReadToEnd();
                            //}
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
                            await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                            res.status = "1";
                            res.message = "Transaction reversa made successfully";
                        }
                        else
                        {
                            res.status = "-1";
                            res.message = "Transaction is not succed please try";
                        }
                        //await updateTransactionReversalStatus(tras.Id, "UnResponsive", "Revesal Process UnResponsive");
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
                    await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, transaction.transactionId);

                    return new Response()
                    {
                        status = "1",
                        message = "Transaction reversa made successfully"
                    };
                }
                else
                {
                    return new Response()
                    {
                        status = "-1",
                        message = "Transaction reversal not succed please try"
                    };
                }

            }
        }


        public async Task<Response> CreateServiceChargeReversal(TransactionReversal tras)
        {
            Transaction transaction = new Transaction();
            transaction.amount = Convert.ToDecimal(tras.Amount);
            transaction.accountCredited = tras.DebitedAccountNumber;
            transaction.branch = tras.branch;
            transaction.transactionId = tras.RefNo;
            transaction.Id = tras.Id;
            string DATA = JsonConvert.SerializeObject(transaction);

            //var checkposibility = await checktransactionposibility(transaction);
            //if (checkposibility.status != "valid")
            //{
            //    return checkposibility;
            //}
            //URL = "https://10.1.22.198:4040/createTransferreverse";
            string URL = _configuration["EndPointUrl:createServiceChargeTransferReverse"];
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
                        await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                        //try
                        //{
                        //UpdateTransaction updateTransaction = new UpdateTransaction();
                        //updateTransaction.acctno = tras.DebitedAccountNumber;
                        //updateTransaction.transid = tras.RefNo;
                        //string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                        //URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/reverseTransactionStatus";
                        //request = (HttpWebRequest)WebRequest.Create(URL);
                        //request.Method = "POST";
                        //request.ContentType = "application/json";
                        //request.ContentLength = updateTransactionData.Length;

                        //}
                        //catch (Exception ex) { }
                        return res;
                    }
                    else
                    {

                        //var validate = await _iCbsTransactionImportRepo.ValidTransactionSuccess(transaction.transactionId);
                        //if (validate.Count > 0)
                        //{
                        //    string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                        //    await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                        //    res.status = "1";
                        //    res.message = "Transaction reversa made successfully";
                        //}
                        //else
                        //{
                        //    res.status = "-1";
                        //    res.message = "Transaction is not succed please try";
                        //}
                        //await updateTransactionReversalStatus(tras.Id, "UnResponsive", "Revesal Process UnResponsive");
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
                    await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, transaction.transactionId);

                    return new Response()
                    {
                        status = "1",
                        message = "Transaction reversa made successfully"
                    };
                }
                else
                {
                    return new Response()
                    {
                        status = "-1",
                        message = "Transaction reversal not succed please try"
                    };
                }

            }
        }


        public async Task<Response> CreateVATReversal(TransactionReversal tras)
        {
            Transaction transaction = new Transaction();
            transaction.amount = Convert.ToDecimal(tras.Amount);
            transaction.accountCredited = tras.DebitedAccountNumber;
            transaction.branch = tras.branch;
            transaction.transactionId = tras.RefNo;
            transaction.Id = tras.Id;
            string DATA = JsonConvert.SerializeObject(transaction);

            //var checkposibility = await checktransactionposibility(transaction);
            //if (checkposibility.status != "valid")
            //{
            //    return checkposibility;
            //}
            //URL = "https://10.1.22.198:4040/createTransferreverse";
            string URL = _configuration["EndPointUrl:createVATTransferReverse"];
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
                        await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                        //try
                        //{
                        //UpdateTransaction updateTransaction = new UpdateTransaction();
                        //updateTransaction.acctno = tras.DebitedAccountNumber;
                        //updateTransaction.transid = tras.RefNo;
                        //string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

                        //URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/reverseTransactionStatus";
                        //request = (HttpWebRequest)WebRequest.Create(URL);
                        //request.Method = "POST";
                        //request.ContentType = "application/json";
                        //request.ContentLength = updateTransactionData.Length;

                        //}
                        //catch (Exception ex) { }
                        return res;
                    }
                    else
                    {

                        //var validate = await _iCbsTransactionImportRepo.ValidTransactionSuccess(transaction.transactionId);
                        //if (validate.Count > 0)
                        //{
                        //    string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                        //    await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

                        //    res.status = "1";
                        //    res.message = "Transaction reversa made successfully";
                        //}
                        //else
                        //{
                        //    res.status = "-1";
                        //    res.message = "Transaction is not succed please try";
                        //}
                        //await updateTransactionReversalStatus(tras.Id, "UnResponsive", "Revesal Process UnResponsive");
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
                    await updateTransactionReversalStatus(tras.Id, "Reverse", "", user, transaction.transactionId);

                    return new Response()
                    {
                        status = "1",
                        message = "Transaction reversa made successfully"
                    };
                }
                else
                {
                    return new Response()
                    {
                        status = "-1",
                        message = "Transaction reversal not succed please try"
                    };
                }

            }
        }


        public async Task<Response> checktransactionposibility(Transaction transaction)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
            try
            {


                CheckStatus checkAcc = new CheckStatus();
                checkAcc.amtt = transaction.amount;
                checkAcc.account = transaction.accountCredited;
                string checkAccpay = JsonConvert.SerializeObject(checkAcc);
                //string URL = "http://10.1.10.90:7000/api/lib/v1/checktransactionposibility";
                string URL = _configuration["EndPointUrl:checktransactionposibility"];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = checkAccpay.Length;

                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(checkAccpay);
                }


                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    Response res = JsonConvert.DeserializeObject<Response>(response);
                    if (res.status != "valid")
                    {
                        res.status = "-1";
                        res.message = "Account Number is not Allowed to create transfere";
                        await updateTransactionReversalStatus(transaction.Id, "UnResponsive", "Account Number is not Allowed to create transfere");
                    }
                    return res;



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


    public class Transaction
    {
        public int Id { get; set; }
        public decimal amount { get; set; }
        public string accountCredited { get; set; }
        public string branch { get; set; }
        public string transactionId { get; set; }
    }
    public class Response121212
    {
        public string status { get; set; }
        public object message { get; set; }
        public string trnsid { get; set; }
    }
    public class CheckStatus
    {
        public decimal amtt { get; set; }
        public string account { get; set; }
    }

    public class UpdateTransaction
    {
        public string acctno { get; set; }
        public string transid { get; set; }
    }
}