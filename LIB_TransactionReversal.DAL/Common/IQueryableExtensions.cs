using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LIB_Usermanagement.DAL
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, int pageSize = 10, int pageIndex = 0) where TModel : class
            => query.Skip(pageIndex * pageSize).Take(pageSize) ;
    }
}
 