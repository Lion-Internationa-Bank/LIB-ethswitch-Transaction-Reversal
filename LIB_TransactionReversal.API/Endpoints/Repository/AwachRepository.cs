using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using DTO;
using IRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace Repository
{
    public class AwachRepository : IAwachRepository
    {
        public List<Transaction> TransactionList = new List<Transaction>();
        private readonly HttpClient _httpClient;

        public AwachRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AwachResponseDTO> CreateAwachTransfer(decimal amount, string accountNo)
        {
            string url = "http://172.16.100.17:8990/AWACH-INT/services"; // Replace with your SOAP endpoint URL
            string messageId = Helper.generateRandomID(35, "msg");
            string TrnsNo = Helper.generateRandomID(10, "trn"); ;

            string soapRequest = $@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:awac=""http://temenos.com/AWACH-INT"" xmlns:fun=""http://temenos.com/FUNDSTRANSFERAWACH"">
                   <soapenv:Header/>
                   <soapenv:Body>
                      <awac:banktoAWACHft>
                         <WebRequestCommon>
                            <!--Optional:-->
                            <company></company>
                          <password>F15i#4YTyrTlib</password>
                             <userName>LIBBNK1</userName>
                         </WebRequestCommon>
                         <OfsFunction>
                            <!--Optional:-->
                            <activityName> </activityName>
                            <!--Optional:-->
                            <assignReason> </assignReason>
                            <!--Optional:-->
                            <dueDate> </dueDate>
                            <!--Optional:-->
                            <extProcess> </extProcess>
                            <!--Optional:-->
                            <extProcessID> </extProcessID>
                            <!--Optional:-->
                            <gtsControl> </gtsControl>
                            <!--Optional:-->
                            <messageId>{messageId}</messageId>
                            <!--Optional:-->
                            <noOfAuth>0</noOfAuth>
                            <!--Optional:-->
                            <owner> </owner>
                            <!--Optional:-->
                            <replace> </replace>
                            <!--Optional:-->
                            <startDate> </startDate>
                            <!--Optional:-->
                            <user> </user>
                         </OfsFunction>
                         <FUNDSTRANSFERAWACHType id="" "">
                            <!--Optional:-->
                            <fun:TRANSACTIONTYPE>AC</fun:TRANSACTIONTYPE>
                            <!--Optional:-->
                            <fun:DEBITACCTNO>{accountNo}</fun:DEBITACCTNO>
                            <!--Optional:-->
                            <fun:DEBITCURRENCY>ETB</fun:DEBITCURRENCY>
                            <!--Optional:-->
                            <fun:DEBITTHEIRREF>{TrnsNo} </fun:DEBITTHEIRREF>
                            <!--Optional:-->
                            <fun:CREDITTHEIRREF>ft324</fun:CREDITTHEIRREF>
                            <!--Optional:-->
                            <fun:CREDITACCTNO>1000000139</fun:CREDITACCTNO>
                            <!--Optional:-->
                            <fun:CREDITCURRENCY>ETB</fun:CREDITCURRENCY>
                            <!--Optional:-->
                            <fun:CREDITAMOUNT>{amount}</fun:CREDITAMOUNT>
                            <!--Optional:-->
                            <fun:gORDERINGCUST g=""1"">
                               <!--Zero or more repetitions:-->
                               <fun:ORDERINGCUST></fun:ORDERINGCUST>
                            </fun:gORDERINGCUST>
                         </FUNDSTRANSFERAWACHType>
                      </awac:banktoAWACHft>
                   </soapenv:Body>
                </soapenv:Envelope>
                ";

            var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();



                XDocument doc = XDocument.Parse(responseString);
                XNamespace ns4 = "http://temenos.com/FUNDSTRANSFER"; // Define the namespace for ns4

                var transactionId = doc.Descendants("transactionId").FirstOrDefault()?.Value;
                var successIndicator = doc.Descendants("successIndicator").FirstOrDefault()?.Value;
                string message = "";
                if (successIndicator == "T24Error")
                {
                    message = doc.Descendants("messages").FirstOrDefault()?.Value;
                }
                var res = new AwachResponseDTO()
                {
                    success = successIndicator == "Success" ? true : false,
                    message = message == "" ? successIndicator : message,
                    awachTransactionId = transactionId,
                    messageId = messageId,
                    referenceNo = TrnsNo
                };
                return res;
            }

            catch (Exception ex)
            {
                var res = new AwachResponseDTO()
                {
                    success = true,
                    message = "pending",
                    awachTransactionId = "",
                    messageId = messageId,
                    referenceNo = TrnsNo
                };
                return res;
            }
        }

        public async Task SendPendingTransaction()
        {
            var pendingTransaction = TransactionList.Where(x => x.status == "pending").ToList();
            foreach (var transaction in pendingTransaction)
            {
                var res = await CreateAwachTransfer(transaction.amount, transaction.ReciverAccountId);
                if (res.success)
                {
                    transaction.status = "success";
                }
                else
                {
                    transaction.status = "pending";
                }
            }
        }


    }

    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
}
