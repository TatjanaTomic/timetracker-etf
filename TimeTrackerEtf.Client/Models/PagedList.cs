﻿using System;
using System.Collections.Generic;

namespace TimeTrackerEtf.Client.Models
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages =>
            (int) Math.Ceiling(TotalCount / (decimal) PageSize);
    }
}