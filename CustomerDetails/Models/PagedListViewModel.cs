using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerDetails.Models
{
    public class PagedListViewModel<T>
    {
        public IEnumerable<T> PagedItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages
        {
            get { return (int)System.Math.Ceiling((double)TotalCount / PageSize); }
        }

        public PagedListViewModel(IEnumerable<T> pagedItems, int pageNumber, int pageSize, int totalCount)
        {
            PagedItems = pagedItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
