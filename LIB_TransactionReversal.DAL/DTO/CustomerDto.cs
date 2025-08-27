using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LIB_Documentmanagement.DAL.DTO
{
    public class CustomerDto
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public string LegalPerson { get; set; }
        public DateTime OpeningDate { get; set; }
        public int? DocumentTypeId { get; set; }
        public IFormFile File { get; set; }

    }
}
