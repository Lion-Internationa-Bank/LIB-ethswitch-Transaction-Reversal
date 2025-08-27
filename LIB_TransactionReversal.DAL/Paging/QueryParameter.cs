using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIB_Usermanagement.Paging
{
    //to create an object with more (e.g entity specfic) attributes, inherit from this class
    public class QueryParameter
{
        public int PageIndex { get; set; }
        public int PageSize { get; set; } 
     
        public QueryParameter()
        {
            this.PageIndex = 0;
            this.PageSize = 10;
       
        }
        public QueryParameter(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        
        }
    }
}
