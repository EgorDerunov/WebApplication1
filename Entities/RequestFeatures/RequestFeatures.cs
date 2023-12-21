﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public abstract class RequestFeatures
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public string OrderBy { get; set; }
        public string Fields { get; set; }
    }
}
