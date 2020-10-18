using System;
using System.Collections;
using System.Collections.Generic;

namespace NovelWorld.Data.Requests
{
    public class PagingRequest: Request
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public IEnumerable<FilterRequest> Filters { get; set; }
        public IEnumerable<SortRequest> Sorts { get; set; }
    }
    
    public class FilterRequest: Request
    {
        public string Field { get; set; }
        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBool { get; set; }
        public DateTime? ValueDateTimeFrom { get; set; }
        public DateTime? ValueDateTimeTo { get; set; }
        public IEnumerable ValueList { get; set; }
    }
    
    public class SortRequest: Request
    {
        public string Field { get; set; }
        public bool Asc { get; set; }
    }
}