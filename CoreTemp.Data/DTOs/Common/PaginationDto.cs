using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreTemp.Data.DTOs.Common
{
    public class PaginationDto
    {
        [Description("شماره صفحه")]
        public int PageNumber { get; set; } = 0;

        private int pageSize = 10;
        [Description("تعداد در هر صفحه")]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > 50) ? 50 : value; }
        }
    }
}
