

using System;

namespace LIB_Usermanagement.DAL.DTO
{
    public class SearchParams : QueryParameters
    {
        public string AccountNo { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string TransactionType { get; set; }
        public string GlAccountNo { get; set; }
    }


}
