using System.Collections.Generic;

namespace LIB_TransactionReversal.API.Endpoints.Repository
{
    public class Envelope
    {
        public Body Body { get; set; }
    }

    public class Body
    {
        public Result Result { get; set; }
    }

    public class Result
    {
        public Header Header { get; set; }
        public BodyResult Body { get; set; }
    }

    public class Header
    {
        public string Version { get; set; }
        public string OriginatorConversationID { get; set; }
        public string ConversationID { get; set; }
    }

    public class BodyResult
    {
        public string ResultType { get; set; }
        public string ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public List<ReferenceItem> ReferenceData { get; set; }
    }

    public class TransactionResult
    {
        public string TransactionID { get; set; }
        public List<ResultParameter> ResultParameters { get; set; }
    }

    public class ResultParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ReferenceItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
