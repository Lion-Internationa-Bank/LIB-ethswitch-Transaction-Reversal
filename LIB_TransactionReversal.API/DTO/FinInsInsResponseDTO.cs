namespace DTO
{
    public class FinInsInsResponseDTO
    {
        public bool success {  get; set; }
        public string message { get; set; } 
        public string FinInstransactionId { get; set; } 
        public string ConversationID { get; set; } 
    }

    public class AwachResponseDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string messageId { get; set; }
        public string referenceNo { get; set; }
        public string awachTransactionId { get; set; }
        public string ConversationID { get; set; }
    }

    public class TelebirrResponseDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string libConversationID { get; set; }
        public string telebirrConversationID { get; set; }
    }

    public class EthswichResponseDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string referenceNumber { get; set; }
    }

    public class MpesaResponseDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string libConversationID { get; set; }
        public string mpesaConversationID { get; set; }
    }
}
