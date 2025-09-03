using LIB_Documentmanagement.DAL.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using LIB_TransactionReversal.Application.Interfaces;
using LIB_Usermanagement.DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using Microsoft.AspNetCore.Authorization;

namespace LIB_TransactionReversal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionAdjustementController : ControllerBase
    {
        private readonly ITransactionAdjustementService _transactionAdjustementService;
        public TransactionAdjustementController(ITransactionAdjustementService transactionAdjustementService)
        {
            _transactionAdjustementService = transactionAdjustementService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveTransactionAdjustement([FromBody] TransactionAdjustement TransactionAdjustement)
        {
            try
            {
                await _transactionAdjustementService.SaveTransactionAdjustement(TransactionAdjustement);
                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdatTransactionAdjustement([FromBody] TransactionAdjustement TransactionAdjustement)
        {
            try
            {
                await _transactionAdjustementService.UpdatTransactionAdjustement(TransactionAdjustement);
                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTransactionAdjustement(int Id)
        {
            try
            {
                await _transactionAdjustementService.GetTransactionAdjustement(Id);
                return StatusCode(200);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionAdjustement([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _transactionAdjustementService.GetTransactionAdjustement(param);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetAdjustementReport")]
        public async Task<IActionResult> GetAdjustementReport([FromQuery] SearchParams param)
        {
            try
            {
                var result = await _transactionAdjustementService.GetAdjustementReport(param);
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
            var result = await _transactionAdjustementService.CreateTransactionAdjustement(Id);
            return Ok(result);

        }
        [HttpPost("CheckedPendingTransactionForReversal")]
        public async Task<IActionResult> CheckedPendingTransactionForReversal([FromBody] List<int> Ids)
        {
            await _transactionAdjustementService.CheckedPendingTransactionForAdjustement(Ids);
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
                result = await _transactionAdjustementService.CreateTransactionAdjustement(id);
                if (result.status == "0")
                {
                    return BadRequest(result);
                }
            }
            return Ok(result);
        }

        [HttpPost("updateTransactionAccountNumber")]
        public async Task<IActionResult> updateTransactionAccountNumber([FromBody] UpdateTransactionAccountDto updateTransactionAccountDto)
        {
           await _transactionAdjustementService.updateTransactionAccountNumber(updateTransactionAccountDto);
            return Ok();
        }


    }
}
