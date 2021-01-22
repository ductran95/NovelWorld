using System;
using System.Collections;
using System.Collections.Generic;

namespace NovelWorld.Data.Requests
{
    public class PagingRequest: IRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public List<FilterRequest> Filters { get; set; }
        public List<SortRequest> Sorts { get; set; }
    }
    
    public class FilterRequest: IRequest
    {
        public string Field { get; set; }
        public string ValueString { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBool { get; set; }
        public DateTime? ValueDateTimeFrom { get; set; }
        public DateTime? ValueDateTimeTo { get; set; }
        public IEnumerable ValueList { get; set; }
    }
    
    public class SortRequest: IRequest
    {
        public string Field { get; set; }
        public bool Asc { get; set; }
    }
}