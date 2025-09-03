using LIB_Documentmanagement.DAL.Entity;
using System.Threading.Tasks;
using System;
using LIB_TransactionReversal.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LIB_Usermanagement.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.DTO;
using System.Linq;
using LIB_Documentmanagement.API.Controllers;
using LIB_TransactionReversal.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace LIB_TransactionReversal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EthswitchTransactionImportController : ControllerBase
    {
        private readonly IEthswitchTransactionImportService _ethswitchTransactionImportService;
        public EthswitchTransactionImportController(IEthswitchTransactionImportService ethswitchTransactionImportService)
        {
            _ethswitchTransactionImportService = ethswitchTransactionImportService;
        }


        [HttpPost]
        public async Task<IActionResult> ImportEthswitchTransaction([FromBody] List<ImportEthswichTransactionDto> EthswitchTransaction)
        {
            try
            {
                EthswitchTransaction.RemoveAll(p => p == null);
                var transRang = EthswitchTransaction.Where(p => p != null).Take(1).FirstOrDefault();
               // var validDate = Convert.ToDateTime(EthswitchTransaction.Where(p => p != null).Select(p => p.TransactionDateFrom).FirstOrDefault());
               // var EthswitchTransaction.Where(p => Convert.ToDateTime(p.Transaction_Date).Date != validDate.Date);
                var transRangExistList = await _ethswitchTransactionImportService.CheckTransactionRangeExist(Convert.ToDateTime(transRang.TransactionDateFrom), Convert.ToDateTime(transRang.TransactionDateTo));
                if (transRangExistList.Count > 0)
                {
                    var res = new Response() { message = "Transaction with selected date already imported", status = "1" };
                    return Ok(res);
                }

                var invalidDate = EthswitchTransaction.Where(p=>Convert.ToDateTime(p.TransactionDateFrom).Date != Convert.ToDateTime(p.Transaction_Date).Date).ToList();
                invalidDate = invalidDate.Where(p => p != null && !string.IsNullOrEmpty(p.Transaction_Date)).ToList();
                if (invalidDate.Count > 0)
                {
                    await _ethswitchTransactionImportService.InsertInvalidDateEthiswichTransaction(invalidDate);
                    //var res = new Response() { message = $"There is Wrong date in the Excel - {invalidDate[0].Transaction_Date}. Can not import please fix the date ", status = "1" };
                   // return Ok(res);
                }
                EthswitchTransaction = EthswitchTransaction.Where(p => Convert.ToDateTime(p.TransactionDateFrom).Date == Convert.ToDateTime(p.Transaction_Date).Date).ToList();
                var result = await _ethswitchTransactionImportService.ImportEthswitchTransaction(EthswitchTransaction);
                if (result.Count == 0)
                {
                    var res = new Response() { message = "No transaction imported", status = "0" };
                    return Ok(res);
                }
                return Ok(result);
                //return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetSuccessfullTransaction")]
        public async Task<IActionResult> GetSuccessfullTransaction([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.GetSuccessfullTransaction(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetTransactionNotFoundLib")]
        public async Task<IActionResult> GetTransactionNotFoundLib([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.GetTransactionNotFoundLib(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetTransactionNotFoundEthswitch")]
        public async Task<IActionResult> GetTransactionNotFoundEthswitch([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.GetTransactionNotFoundEthswitch(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        } 
        
        [HttpGet("ReconsilePendingTransaction")]
        public async Task<IActionResult> ReconsilePendingTransaction([FromQuery]  SearchParams searchParams)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.ReconsilePendingTransaction(searchParams);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }


        [HttpGet("GetImportedTransaction")]
        public async Task<IActionResult> GetImportedTransaction([FromQuery] SearchParams searchParams)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.GetImportedTransaction(searchParams);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("ReconsillationSummaryReport")]
        public async Task<IActionResult> ReconsillationSummaryReport([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.ReconsillationSummaryReport(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetInvalidEthiswichDateTransaction")]
        public async Task<IActionResult> GetInvalidEthiswichDateTransaction([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.GetInvalidEthiswichDateTransaction(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetAccountHolderName/{account}")]
        public async Task<IActionResult> GetAccountHolderName(string account)
        {
            try
            {
                var result = await _ethswitchTransactionImportService.GetAccountDetail(account);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
    }
}
