using LIB_TransactionReversal.API.AwachTestModels;
using System.IO;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace LIB_TransactionReversal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoapRequestController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public SoapRequestController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("CreateTransfer")]
        public async Task<IActionResult> CreateTransfer()
        {
            var transferRequest = new TransferRequest
            {
                Envelope = new Envelope
                {
                    Header = new Header(),
                    Body = new Body
                    {
                        CreateTransferRequestFlow = new CreateTransferRequestFlow
                        {
                            RequestHeader = new RequestHeader
                            {
                                RequestId = "SCH13a642255695946580a41aa603cb3c89",
                                ServiceName = "createTransfer",
                                Timestamp = "2024-11-26T05:47:14.127Z",
                                OriginalName = "AWACHAPP",
                                LanguageCode = "002",
                                UserCode = "AWACH"
                            },
                            CreateTransferRequest = new CreateTransferRequest
                            {
                                Canal = "AWACH_OUT",
                                Pain001 = "<![CDATA[<Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn:iso:std:iso:20022:tech:xsd:pain.001.001.03DB\"> ... </Document>]]>"
                            }
                        }
                    }
                }
            };

            var xmlSerializer = new XmlSerializer(typeof(TransferRequest));
            string soapEnvelope;

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, transferRequest);
                soapEnvelope = stringWriter.ToString();
            }

            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            var response = await _httpClient.PostAsync("https://testapi/create", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }

            //////////////////////////////////////////////////////////////
            ///
            Document document = new Document();
            XmlSerializer serializer = new XmlSerializer(typeof(Document));

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, document);
                string xmlOutput = writer.ToString();
                Console.WriteLine(xmlOutput);
            }
            //////////////////////////////////////////////////////////////

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }


        [HttpGet("AwachNameCheck/{memberId}")]
        public async Task<IActionResult> AwachNameCheck(string memberId)
        {
            string url = "http://172.16.100.17:8990/AWACH-INT/services"; // Replace with your SOAP endpoint URL
            string company = "LIB"; // Replace with actual company value
            string password = "YOUR_PASSWORD"; // Replace with actual password
            string userName = "LIBBNK1"; // Replace with actual username
            string criteriaValue = "memberId"; // Replace with actual criteria value

            string soapRequest = $@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:awac=""http://temenos.com/AWACH-INT"">
                <soapenv:Header />
                <soapenv:Body>
                    <awac:ACCTENQ>
                        <WebRequestCommon xmlns="""">
                            <company>{company}</company>
                            <password>{password}</password>
                            <userName>{userName}</userName>
                        </WebRequestCommon>
                        <ACCTType xmlns="""">
                            <enquiryInputCollection>
                                <columnName>ID</columnName>
                                <criteriaValue>{criteriaValue}</criteriaValue>
                                <operand>EQ</operand>
                            </enquiryInputCollection>
                        </ACCTType>
                    </awac:ACCTENQ>
                </soapenv:Body>
            </soapenv:Envelope>";

            var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                return Ok(responseString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }
        }

        [HttpPost("CreateTransfer2")]
        public async Task<IActionResult> CreateTransfer2()
        {
            //  ServicePointManager.ServerCertificateValidationCallback =
            //(sender, certificate, chain, sslPolicyErrors) => true;
           
           
            string url = "https://aif-uat-srv:8095/createTransfer"; 
            string requestId = Guid.NewGuid().ToString(); 
            string timestamp = DateTime.Now.ToString(); 
            string MsgId = Guid.NewGuid().ToString();
            string InstdAmt = "1";
            //string CtrlSum = "1"; 
            string PmtInfId = Guid.NewGuid().ToString();
            string Id = Guid.NewGuid().ToString();
            string InstrId = Guid.NewGuid().ToString();
            string EndToEndId = Guid.NewGuid().ToString();
            string Ustrd = "Awatest0001";
            string DbtrAcctId = "00311901248";
            string CdtrAcctId = "00311925933";
            string DbtrAgtBrnchId = "00003";
            string CdtrAgtBrnchId = "00189"; 

            string soapRequest = $@"
           <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:amp=""http://soprabanking.com/amplitude"" >
                <soapenv:Header />
                <soapenv:Body >
                <amp:createTransferRequestFlow >
                <amp:requestHeader >
                <amp:requestId >${requestId}</amp:requestId>
                <amp:serviceName >createTransfer</amp:serviceName>
                <amp:timestamp >${timestamp}</amp:timestamp>
                <amp:originalName >AWACHAPP</amp:originalName>
                <amp:languageCode >002</amp:languageCode>
                <amp:userCode >AWACH</amp:userCode>
                </amp:requestHeader>
                <amp:createTransferRequest >
                <amp:canal >AWACH_OUT</amp:canal>
                <amp:pain001 ><![CDATA[ 
                 <Document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.03DB""> 
                  <CstmrCdtTrfInitn> 
                    <GrpHdr> 
                      <MsgId>${MsgId}</MsgId> 
                      <CreDtTm>${timestamp}</CreDtTm> 
                      <NbOfTxs>1</NbOfTxs> 
                      <CtrlSum>${InstdAmt}</CtrlSum> 
                      <InitgPty/> 
                      <DltPrvtData> 
                        <FlwInd>PROD</FlwInd> 
                        <DltPrvtDataDtl> 
                          <PrvtDtInf>AWACH_OUT</PrvtDtInf> 
                          <Tp> 
                            <CdOrPrtry> 
                              <Cd>CHANNEL</Cd> 
                            </CdOrPrtry> 
                          </Tp> 
                        </DltPrvtDataDtl> 
                      </DltPrvtData> 
                    </GrpHdr> 
                    <PmtInf> 
                      <PmtInfId>${PmtInfId}</PmtInfId> 
                      <PmtMtd>TRF</PmtMtd> 
                      <BtchBookg>0</BtchBookg> 
                      <NbOfTxs>1</NbOfTxs> 
                      <CtrlSum>${InstdAmt}</CtrlSum> 
                      <DltPrvtData> 
                        <OrdrPrties> 
                          <Tp>IMM</Tp> 
                          <Md>CREATE</Md> 
                        </OrdrPrties> 
                      </DltPrvtData> 
                      <PmtTpInf> 
                        <InstrPrty>NORM</InstrPrty> 
                        <SvcLvl> 
                          <Prtry>INTERNAL</Prtry> 
                        </SvcLvl> 
                      </PmtTpInf> 
                      <ReqdExctnDt>1901-01-01</ReqdExctnDt> 
                      <Dbtr> 
                      </Dbtr> 
                      <DbtrAcct> 
                        <Id> 
                          <Othr> 
                            <Id>${DbtrAcctId}</Id> 
                            <SchmeNm> 
                              <Prtry>BKCOM_ACCOUNT</Prtry> 
                            </SchmeNm> 
                          </Othr> 
                        </Id> 
                        <Ccy>ETB</Ccy> 
                      </DbtrAcct> 
                      <DbtrAgt> 
                        <FinInstnId> 
                          <Nm>BANQUE</Nm> 
                          <Othr> 
                            <Id>00011</Id> 
                            <SchmeNm> 
                              <Prtry>ITF_DELTAMOP_IDETAB</Prtry> 
                            </SchmeNm> 
                          </Othr> 
                        </FinInstnId> 
                        <BrnchId> 
                          <Id>${DbtrAgtBrnchId}</Id> 
                          <Nm>Agence</Nm> 
                        </BrnchId> 
                      </DbtrAgt> 
                      <CdtTrfTxInf> 
                        <PmtId> 
                          <InstrId>${InstrId}</InstrId> 
                          <EndToEndId>${EndToEndId}</EndToEndId> 
                        </PmtId> 
                        <Amt> 
                          <InstdAmt Ccy=""ETB"">${InstdAmt}</InstdAmt> 
                        </Amt> 
                        <CdtrAgt> 
                          <FinInstnId> 
                            <Nm>BANQUE</Nm> 
                            <Othr> 
                              <Id>00011</Id> 
                              <SchmeNm> 
                                <Prtry>ITF_DELTAMOP_IDETAB</Prtry> 
                              </SchmeNm> 
                            </Othr> 
                          </FinInstnId> 
                          <BrnchId> 
                            <Id>${CdtrAgtBrnchId}</Id> 
                            <Nm>Agence</Nm> 
                          </BrnchId> 
                        </CdtrAgt> 
                        <Cdtr> 
                        </Cdtr> 
                        <CdtrAcct> 
                          <Id> 
                            <Othr> 
                              <Id>${CdtrAcctId}</Id>
                              <SchmeNm> 
                                <Prtry>BKCOM_ACCOUNT</Prtry> 
                              </SchmeNm> 
                            </Othr> 
                          </Id> 
                          <Ccy>ETB</Ccy> 
                        </CdtrAcct> 
                        <RmtInf> 
                          <Ustrd>${Ustrd}</Ustrd> 
                        </RmtInf> 
                      </CdtTrfTxInf> 
                    </PmtInf> 
                  </CstmrCdtTrfInitn> 
                </Document> 
                ]]></amp:pain001>
                </amp:createTransferRequest>
                </amp:createTransferRequestFlow>
                </soapenv:Body>
                </soapenv:Envelope>";

            var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

            try
            {

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                return Ok(responseString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }
        }


        //public static string GenerateGUIDWithHyphens()
        //{
        //    var guidTemplate = "xxxxxxxx-xxxx-4xxxyxxxxxxxxxxxxxxx";
        //    Random random = new Random();
        //    string guid = guidTemplate.Replace('x',  random.Next(0, 16).ToString("x"))
        //                              .Replace('y', _ => (random.Next(0, 4) | 8).ToString("x"));
        //    return guid.Replace("-", "");
        //}
    }




    public class BeneficiaryNameEnquiries
    {
        public string beneficiaryAccountId { get; set; }
        public string beneficiaryName { get; set; }
        public string beneficiaryAccountIdType { get; set; }
        public Guid referenceId { get; set; }
        public string paymentcategory { get; set; }
        public string paymentScheme { get; set; }
        public string financialInstitutionBicCode { get; set; }
        public string financialInstitutionName { get; set; }
        public string status { get; set; }
        public DateTime EventDate { get; set; }


    }


    public class Transaction
    {
        public Guid accountId { get; set; }
        public Guid reservationId { get; set; }
        public Guid referenceId { get; set; }
        public decimal amount { get; set; }
        public DateTime requestedExecutionDate { get; set; }
        public string paymentType { get; set; }
        public string paymentScheme { get; set; }
        public string paymentMethod { get; set; }
        public string ReciverAccountId { get; set; }
        public string ReciverAccountIdType { get; set; }
        public string bankId { get; set; }
        public string bankIdType { get; set; }
        public string bankName { get; set; }
        public string status { get; set; }
        public string cbsStatusMessage { get; set; }
        public string bankStatusMessage { get; set; }

    }
    public class ErrorLog
    {
        public Guid ticketId { get; set; }
        public string traceId { get; set; }
        public string returnCode { get; set; }
        public DateTime EventDate { get; set; }
        public string feedbacks {  get; set; }
    }
}