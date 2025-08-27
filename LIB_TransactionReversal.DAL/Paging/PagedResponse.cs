using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIB_Usermanagement.Paging
{
    public class PagedResponse<T>  
    {
        public T Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
     

        public PagedResponse(T data, int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Data = data;
          
        }
    }
}
