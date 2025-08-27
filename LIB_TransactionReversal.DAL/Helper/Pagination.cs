using LIB_Usermanagement.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIB_Usermanagement.DAL.Helper
{
    public static class Pagination<T>

    {
        public static PaginationModel GetPaginationData(PagedList<T> pagination)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.HasPrevious = pagination.HasPrevious;
            paginationModel.PageSize = pagination.PageSize;
            paginationModel.CurrentPage = pagination.CurrentPage;
            paginationModel.TotalCount = pagination.TotalCount;
            paginationModel.HasNext = pagination.HasNext;
            paginationModel.TotalPages = pagination.TotalPages;
            return paginationModel;
        }

      
    }
}
