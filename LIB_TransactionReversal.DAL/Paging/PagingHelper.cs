using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIB_Usermanagement.Paging
{
    public static class PagingHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, QueryParameter qParam, int totalCount)
        {
            var respose = new PagedResponse<List<T>>(pagedData, qParam.PageIndex, qParam.PageSize);
            var totalPages = Convert.ToInt32((((double)totalCount / (double)qParam.PageSize))); // may not be required now
            respose.TotalPages = totalPages;
            respose.TotalCount = totalCount;
            return respose;
        }
    }
}
