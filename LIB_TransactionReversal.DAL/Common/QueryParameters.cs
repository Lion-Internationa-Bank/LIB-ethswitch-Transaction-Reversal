using System;
using System.Collections.Generic;
using System.Text;

namespace LIB_Usermanagement.DAL
{
    public class QueryParameters
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        //public int PageSize { get; set; }
        private int _pageSize { get; set; } = 100;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

    }
}
