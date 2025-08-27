using System.Collections.Generic;
using System.Xml.Serialization;

namespace DTO
{
    [XmlRoot(ElementName = "Status", Namespace = "http://temenos.com/AWACH-INT")]
    public class Status
    {
        public string TransactionId { get; set; }

        public string MessageId { get; set; }

        public string SuccessIndicator { get; set; }

        public string Application { get; set; }
    }

    [XmlRoot(ElementName = "FUNDSTRANSFERType", Namespace = "http://temenos.com/AWACH-INT")]
    public class FundsTransferType
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "TRANSACTIONTYPE", Namespace = "http://temenos.com/FUNDSTRANSFER")]
        public string TransactionType { get; set; }

        [XmlElement(ElementName = "DEBITACCTNO", Namespace = "http://temenos.com/FUNDSTRANSFER")]
        public string DebitAccountNo { get; set; }

        // Add other properties similarly...
    }

    [XmlRoot(ElementName = "banktoAWACHftResponse", Namespace = "http://temenos.com/AWACH-INT")]
    public class BankToAwachFtResponse
    {
        [XmlElement(ElementName = "Status")]
        public Status Status { get; set; }

        [XmlElement(ElementName = "FUNDSTRANSFERType")]
        public FundsTransferType FundsTransferType { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "banktoAWACHftResponse", Namespace = "http://temenos.com/AWACH-INT")]
        public BankToAwachFtResponse BankToAwachFtResponse { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }
    }
}