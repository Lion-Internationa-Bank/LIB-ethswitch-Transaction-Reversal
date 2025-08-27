using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Documentmanagement.DAL.DTO
{
    public class CustomerDocumentListDto
    {
        public string Link { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }
        public bool Uploaded { get; set; }
        public int Id { get; set; }
        public bool IsRequired { get; set; }

        public int DocumentId { get; set; }
    }
}
