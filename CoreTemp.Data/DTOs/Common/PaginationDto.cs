using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DTOs.Common
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 0;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > 50) ? 50 : value; }
        }
    }
}
