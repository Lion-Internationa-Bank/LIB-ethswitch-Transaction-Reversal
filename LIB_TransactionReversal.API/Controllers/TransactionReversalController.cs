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
                if(result.status == "0")
                {
                    return BadRequest(result);
                }
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

