using LIB_Documentmanagement.DAL.Entity;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using LIB_Documentmanagement.Application.Interfaces;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using LIB_Usermanagement.DAL.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using LIB_TransactionReversal.DAL.DTO;

namespace LIB_Documentmanagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionReversalController : ControllerBase
    {
        private readonly ITransactionReversalService _transactionReversalService;
        private readonly IConfiguration _configuration;

        public TransactionReversalController(ITransactionReversalService transactionReversalService,
            IConfiguration configuration)
        {
            _transactionReversalService = transactionReversalService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SaveTransactionReversal([FromBody] TransactionReversal TransactionReversal)
        {
            try
            {
                await _transactionReversalService.SaveTransactionReversal(TransactionReversal);
                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdatTransactionReversal([FromBody] TransactionReversal TransactionReversal)
        {
            try
            {
                await _transactionReversalService.UpdatTransactionReversal(TransactionReversal);
                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTransactionReversal(int Id)
        {
            try
            {
                await _transactionReversalService.GetTransactionReversal(Id);
                return StatusCode(200);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionReversal([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _transactionReversalService.GetTransactionReversal(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetReversalReport")]
        public async Task<IActionResult> GetReversalReport([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _transactionReversalService.GetReversalReport(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("CreateTransaction/{Id}")]
        public async Task<IActionResult> CreateTransaction(int Id)
        {
            var result = await _transactionReversalService.CreateTransactionReversal(Id);
            return Ok(result);
           // ServicePointManager.ServerCertificateValidationCallback =
           //(sender, certificate, chain, sslPolicyErrors) => true;
           // try
           // {
           //     var tras = await _transactionReversalService.GetTransactionReversal(Id);
           //     Transaction transaction = new Transaction();
           //     transaction.amount = Convert.ToDecimal(tras.TotalAmount);
           //     transaction.accountCredited = tras.AccountNumber;
           //     transaction.branch = tras.branch;
           //     transaction.transactionId = tras.rrn;
           //     string DATA = JsonConvert.SerializeObject(transaction);

           //     CheckStatus checkAcc = new CheckStatus();
           //     checkAcc.amtt = tras.Amount;
           //     checkAcc.account = tras.AccountNumber;
           //     string checkAccpay = JsonConvert.SerializeObject(checkAcc);
           //     //string URL = "http://10.1.10.90:7000/api/lib/v1/checktransactionposibility";
           //     string URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/checktransactionposibility";
           //     HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
           //     request.Method = "POST";
           //     request.ContentType = "application/json";
           //     request.ContentLength = checkAccpay.Length;

           //     using (Stream webStream = request.GetRequestStream())
           //     using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
           //     {
           //         requestWriter.Write(checkAccpay);
           //     }


           //     WebResponse webResponse = request.GetResponse();
           //     using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
           //     using (StreamReader responseReader = new StreamReader(webStream))
           //     {
           //         string response = responseReader.ReadToEnd();
           //         Response res = JsonConvert.DeserializeObject<Response>(response);
           //         if (res.status != "valid")
           //         {
           //             await _transactionReversalService.updateTransactionReversalStatus(tras.Id, "UnResponsive", "Account Number is not Allowed to create transfere");
           //             return Ok(res);
           //         }
                   
           //     }
            


           //     //URL = "https://10.1.22.198:4040/createTransferreverse";
           //     URL = _configuration["EndPointUrl:CreateTransfere"] + "/createTransferreverse";
           //     request = (HttpWebRequest)WebRequest.Create(URL);
           //     request.Method = "POST";
           //     request.ContentType = "application/json";
           //     request.ContentLength = DATA.Length;
           //     try
           //     {
           //         using (Stream webStream = request.GetRequestStream())
           //         using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
           //         {
           //             requestWriter.Write(DATA);
           //         }


           //         webResponse = request.GetResponse();
           //         using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
           //         using (StreamReader responseReader = new StreamReader(webStream))
           //         {
           //             string response = responseReader.ReadToEnd();
           //             Response res = JsonConvert.DeserializeObject<Response>(response);
           //             if (res.status == "1")
           //             {
           //                 string user = User.Identity.Name;
           //                 await _transactionReversalService.updateTransactionReversalStatus(tras.Id, "Reverse", "", user, res.trnsid);

           //                 try
           //                 {
           //                     UpdateTransaction updateTransaction = new UpdateTransaction();
           //                     updateTransaction.acctno = tras.AccountNumber;
           //                     updateTransaction.transid = tras.rrn;
           //                     string updateTransactionData = JsonConvert.SerializeObject(updateTransaction);

           //                     URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/reverseTransactionStatus";
           //                     request = (HttpWebRequest)WebRequest.Create(URL);
           //                     request.Method = "POST";
           //                     request.ContentType = "application/json";
           //                     request.ContentLength = updateTransactionData.Length;
           //                     using (Stream updatewebStream = request.GetRequestStream())
           //                     using (StreamWriter requestWriter = new StreamWriter(updatewebStream, System.Text.Encoding.ASCII))
           //                     {
           //                         requestWriter.Write(updateTransactionData);
           //                     }
           //                     webResponse = request.GetResponse(); using (Stream updatewebStream = webResponse.GetResponseStream() ?? Stream.Null)
           //                     using (StreamReader updateresponseReader = new StreamReader(updatewebStream))
           //                     {
           //                         string updateresponse = updateresponseReader.ReadToEnd();
           //                     }
           //                 }
           //                 catch (Exception ex){ }
           //                 return Ok(res);
           //             }
           //             else
           //             {
           //                 await _transactionReversalService.updateTransactionReversalStatus(tras.Id, "UnResponsive", "Revesal Process UnResponsive");
           //                 return Ok(res);

           //             }

           //         }
           //     } catch
           //     {
           //         await _transactionReversalService.updateTransactionReversalStatus(tras.Id, "UnResponsive", "Revesal Process UnResponsive");
           //         return Ok(new Response()
           //         {
           //             status = "0",
           //             message = "UnResponsive"
           //         });
           //     }
           // }

           // catch (Exception ex)
           // {
           //     return BadRequest(ex.Message);

           // }

        }
        [HttpPost("CheckedPendingTransactionForReversal")]
        public async Task<IActionResult> CheckedPendingTransactionForReversal([FromBody] List<int> Ids)
        {
            await _transactionReversalService.CheckedPendingTransactionForReversal(Ids);
            return Ok(new Response()
            {
                status = "1",
                message = "Successfull"
            });
        }

        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] List<int> Ids)
        {
            Response result = new Response();
            foreach (int id in Ids)
            {
                result = await _transactionReversalService.CreateTransactionReversal(id);
            }
            return Ok(result);
        }



        }

    public class Transaction
    {
        public decimal amount { get; set; }
        public string accountCredited { get; set; }
        public string branch { get; set; }
        public string transactionId { get; set; }
    }
    public class Response1212
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

