using System.Xml.Serialization;


namespace LIB_TransactionReversal.API.AwachTestModels
{
    // Models/TransferRequest.cs

    public class TransferRequest
    {
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Envelope Envelope { get; set; }
    }

    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement("Header")]
        public Header Header { get; set; }

        [XmlElement("Body")]
        public Body Body { get; set; }
    }

    public class Header { }

    public class Body
    {
        [XmlElement(Namespace = "http://soprabanking.com/amplitude")]
        public CreateTransferRequestFlow CreateTransferRequestFlow { get; set; }
    }

    public class CreateTransferRequestFlow
    {
        [XmlElement(Namespace = "http://soprabanking.com/amplitude")]
        public RequestHeader RequestHeader { get; set; }

        [XmlElement(Namespace = "http://soprabanking.com/amplitude")]
        public CreateTransferRequest CreateTransferRequest { get; set; }
    }

    public class RequestHeader
    {
        public string RequestId { get; set; }
        public string ServiceName { get; set; }
        public string Timestamp { get; set; }
        public string OriginalName { get; set; }
        public string LanguageCode { get; set; }
        public string UserCode { get; set; }
    }

    public class CreateTransferRequest
    {
        public string Canal { get; set; }

        [XmlElement(DataType = "string")]
        public string Pain001 { get; set; }
    }
}