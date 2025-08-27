using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_TransactionReversal.DAL.DTO
{
    public class Response
    {
        public string status { get; set; }
        public object message { get; set; }
        public string trnsid { get; set; }
    }
}
