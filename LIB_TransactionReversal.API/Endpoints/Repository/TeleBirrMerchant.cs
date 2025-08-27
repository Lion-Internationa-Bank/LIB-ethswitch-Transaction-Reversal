using DTO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;
using System.Linq;

namespace LIB_TransactionReversal.API.Endpoints.Repository
{
    public class TeleBirrMerchant
    {
        private readonly HttpClient _httpClient;

        public TeleBirrMerchant(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TelebirrResponseDTO> MerchantNameCheck(string Identifier, string Remark)
        {
            string url = "http://10.180.79.13:30001/payment/services/APIRequestMgrService";
            var timestamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
             Identifier = "2000";
             Remark = "test";
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://10.180.79.13:30001/payment/services/SYNCAPIRequestMgrService");
                
                var content = new StringContent(@$"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cps.huawei.com/synccpsinterface/api_requestmgr"" xmlns:req=""http://cps.huawei.com/synccpsinterface/request"" xmlns:com=""http://cps.huawei.com/synccpsinterface/common"">
                       <soapenv:Header/>
                       <soapenv:Body>
                          <api:Request>
                             <req:Header>
                                <req:Version>1.0</req:Version>
                                <req:CommandID>QueryOrganizationInfo</req:CommandID>
                               <req:Caller>
                                   <req:CallerType>2</req:CallerType>
                                   <req:ThirdPartyID>BankOrgQuery</req:ThirdPartyID>
                                   <req:Password>ywbOSX/InhOyFJqaTeG/+2rAoBVAuAqQe/OEgufpmJc=</req:Password>
                                </req:Caller>
                                <req:KeyOwner>1</req:KeyOwner>
                                <req:Timestamp>{timestamp}</req:Timestamp>
                             </req:Header>
                             <req:Body>
                                <req:Identity>
                                   <req:Initiator>
                                      <req:IdentifierType>14</req:IdentifierType>
                                      <req:Identifier>Bank</req:Identifier>
                                      <req:SecurityCredential>UypZ9RlsWAw9nL6vuOvlR4NuNJV2V9Rvcnzdm4UW+dQ=</req:SecurityCredential>
                                   </req:Initiator>
                                   <req:ReceiverParty>
                                      <req:IdentifierType>4</req:IdentifierType>
                                      <req:Identifier>{Identifier}</req:Identifier>
                                   </req:ReceiverParty>
                                </req:Identity>
                                <req:QueryOrganizationInfoRequest/>
                                <req:Remark>test</req:Remark>
                             </req:Body>
                          </api:Request>
                       </soapenv:Body>
                    </soapenv:Envelope>", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();


                string responseString = await response.Content.ReadAsStringAsync();
               

                XDocument doc = XDocument.Parse(responseString);
                XNamespace res = "http://cps.huawei.com/synccpsinterface/result"; // Define the namespace for ns4

                var OrganizationName = doc.Descendants(res+"OrganizationName").FirstOrDefault()?.Value;
                var ShortCode = doc.Descendants(res + "ShortCode").FirstOrDefault()?.Value;
                var IdentityStatus = doc.Descendants(res+"IdentityStatus").FirstOrDefault()?.Value;
                var ResultCode = doc.Descendants(res + "ResultCode").FirstOrDefault()?.Value;
                var ResultType = doc.Descendants(res + "ResultType").FirstOrDefault()?.Value;
                string message = "";

                var ressponse = new TelebirrResponseDTO()
                {
                    //success = ResponseCode == "0" ? true : false,
                    //message = ResponseDesc,
                    //libConversationID = libconversationId,
                    telebirrConversationID = OrganizationName
                };

                return ressponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }
        }


        public async Task<TelebirrResponseDTO> MerchantTransfere(decimal amount, string Identifier)
        {
            string url = "http://10.180.79.13:30001/payment/services/APIRequestMgrService";
            var timestamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
            string callBackUrl = "http://10.180.79.13:30001/mockAPIRequestMgrBinding";
            Identifier = "2000";
            amount = 1.01M;
            string OriginatorConversationID = "VF20241031021200test001";
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://10.180.79.13:30001/payment/services/SYNCAPIRequestMgrService");
                var content = new StringContent(@$"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:api=""http://cps.huawei.com/synccpsinterface/api_requestmgr"" xmlns:req=""http://cps.huawei.com/synccpsinterface/request"" xmlns:com=""http://cps.huawei.com/synccpsinterface/common"">
                           <soapenv:Header/>
                           <soapenv:Body>
                                     <api:Request>
                                 <req:Header>
                                    <req:Version>1.0</req:Version>
                                    <req:CommandID>InitTrans_DepositfromBankOrg</req:CommandID>
                                    <req:OriginatorConversationID>{OriginatorConversationID}</req:OriginatorConversationID>
                                    <req:Caller>
                                       <req:CallerType>2</req:CallerType>
                                       <req:ThirdPartyID>Lion Bank</req:ThirdPartyID>
                                       <req:Password>a7weQz8/EG6bd8iuL4sV9ksPJva+yxwRL4MV2qh0L9Y=</req:Password>
                                    </req:Caller>
                                    <req:KeyOwner>1</req:KeyOwner>
                                    <req:Timestamp>{timestamp}</req:Timestamp>
                                 </req:Header>
                                 <req:Body>
                                    <req:Identity>
                                       <req:Initiator>
                                          <req:IdentifierType>12</req:IdentifierType>
                                          <req:Identifier>002701</req:Identifier>
                                          <req:SecurityCredential>IXUGeVKebKHvOSPdSJPsZiZ6RwN6AATZW3rHElVTe7A=</req:SecurityCredential>
                                          <req:ShortCode>0027</req:ShortCode>
                                       </req:Initiator>
                                       <req:ReceiverParty>
                                          <req:IdentifierType>4</req:IdentifierType>
                                          <req:Identifier>{Identifier}</req:Identifier>
                                       </req:ReceiverParty>
                                    </req:Identity>
                                    <req:TransactionRequest>
                                       <req:Parameters>
                                          <req:Amount>{amount}</req:Amount>
                                          <req:Currency>ETB</req:Currency>
                
                                       </req:Parameters>
                                    </req:TransactionRequest>
                                    <req:Remark>for test</req:Remark>
                                 </req:Body>
                              </api:Request>
                           </soapenv:Body>
                        </soapenv:Envelope>", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();


                string responseString = await response.Content.ReadAsStringAsync();


                XDocument doc = XDocument.Parse(responseString);
                XNamespace res = "http://cps.huawei.com/synccpsinterface/result"; // Define the namespace for ns4

                var OrganizationName = doc.Descendants(res + "OrganizationName").FirstOrDefault()?.Value;
                var ShortCode = doc.Descendants(res + "ShortCode").FirstOrDefault()?.Value;
                var IdentityStatus = doc.Descendants(res + "IdentityStatus").FirstOrDefault()?.Value;
                var ResultCode = doc.Descendants(res + "ResultCode").FirstOrDefault()?.Value;
                var ResultType = doc.Descendants(res + "ResultType").FirstOrDefault()?.Value;
                string message = "";

                var ressponse = new TelebirrResponseDTO()
                {
                    //success = ResponseCode == "0" ? true : false,
                    //message = ResponseDesc,
                    //libConversationID = libconversationId,
                    telebirrConversationID = OrganizationName
                };

                return ressponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw ex;
            }
        }
    }
}
