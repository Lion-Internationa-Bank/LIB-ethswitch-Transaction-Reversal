using System.IO;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LIB_TransactionReversal.API.Endpoints.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Xml.Linq;
using System.Linq;

namespace LIB_TransactionReversal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly AwachRepository _awachRepository;
        private readonly TelebirrRepository _telebirrRepository;
        private readonly EthswichRepository _ethswichRepository;
        private readonly MpesaRepository _mpesaRepository;
        private readonly TeleBirrMerchant _teleBirrMerchant;

        public TestApiController(AwachRepository awachRepository, TelebirrRepository telebirrRepository,
            EthswichRepository ethswichRepository, MpesaRepository mpesaRepository, TeleBirrMerchant teleBirrMerchant)
        {
            _awachRepository = awachRepository;
            _telebirrRepository = telebirrRepository;
            _ethswichRepository = ethswichRepository;
            _mpesaRepository = mpesaRepository;
            _teleBirrMerchant = teleBirrMerchant;
        }

        [HttpGet("CreateAwachTransfer/{amount}/{accountno}")]
        public async Task<IActionResult> CreateAwachTransfer(decimal amount, string accountno)
        {
            //var result = await _awachRepository.CreateAwachTransfer(1, 1000648929);
            var result = await _awachRepository.CreateAwachTransfer(amount, accountno);
            return Ok(result);
        }

        [HttpGet("CreateTelebirrTransaction/{amount}/{phonenum}")]
        public async Task<IActionResult> CreateTelebirrTransaction(decimal amount, string phonenum)
        {
            //var result = await _telebirrRepository.CreateTelebirrTransaction(1, 0911500391);
            var result = await _telebirrRepository.CreateTelebirrTransaction(amount, phonenum);
            return Ok(result);
        }

        [HttpGet("CreateEthswichTransaction")]
        public async Task<IActionResult> CreateEthswichTransaction()
        {
            var result = await _ethswichRepository.CreateEthswichTransaction(1, "1000191505868", "231402");
            return Ok(result);
        }

        [HttpGet("CreateMpesaTransfer/{amount}/{phonenum}")]
        public async Task<IActionResult> CreateMpesaTransfer(decimal amount, string phonenum)
        {
            await _teleBirrMerchant.MerchantTransfere(87765,"");
            //var result = await _mpesaRepository.CreateMpesaTransfer(1, "251700100150");
            var result = await _mpesaRepository.CreateMpesaTransfer(amount, phonenum);
            return Ok(result);
        }

        [HttpGet("MerchantNameCheck/{Identifier}/{Remark}")]
        public async Task<IActionResult> MerchantNameCheck(string Identifier, string Remark)
        {
            var result =  await _teleBirrMerchant.MerchantNameCheck(Identifier, Remark);
            return Ok(result);
        }

        [HttpGet("MerchantTransfere/{Identifier}/{amount}")]
        public async Task<IActionResult> MerchantTransfere(string Identifier, string amount)
        {
            var result = await _teleBirrMerchant.MerchantTransfere(1, Identifier);
            return Ok(result);
        }

        [HttpPost("CallBackResponse")]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public IActionResult CallBackResponse()
        {

           
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var xmlContent = reader.ReadToEnd();

                // Deserialize the XML content
                var serializer = new XmlSerializer(typeof(Envelope));

                //using (var stringReader = new StringReader(xmlContent))
                //{
                //    Envelope envelope;
                //    try
                //    {
                //        envelope = (Envelope)serializer.Deserialize(stringReader);
                //    }
                //    catch (InvalidOperationException)
                //    {
                //        return BadRequest("Invalid SOAP payload.");
                //    }

                //    if (envelope?.Body?.Result != null)
                //    {
                //        // Process the result
                //        return Ok("SOAP request processed successfully.");
                //    }
                //}

                XDocument doc = XDocument.Parse(xmlContent);
                XNamespace res = "http://cps.huawei.com/cpsinterface/result"; // Define the namespace for ns4
                var OriginatorConversationID = doc.Descendants(res + "OriginatorConversationID").FirstOrDefault()?.Value;
                var ConversationID = doc.Descendants(res + "ConversationID").FirstOrDefault()?.Value;
                var ResultType = doc.Descendants(res + "ResultType").FirstOrDefault()?.Value;
                var TransactionID = doc.Descendants(res + "TransactionID").FirstOrDefault()?.Value;
            }

            return BadRequest("Invalid SOAP payload.");
        }

    }
}
