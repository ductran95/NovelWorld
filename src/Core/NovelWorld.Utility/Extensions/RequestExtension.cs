using System.Linq;
using NovelWorld.Data.Requests;

namespace NovelWorld.Utility.Extensions
{
    public static class RequestExtension
    {
        public static void RemoveFilter(this PagingRequest request, FilterRequest filter)
        {
            var list = request.Filters.ToList();
            list.Remove(filter);
            request.Filters = list;
        }

        public static void RemoveSort(this PagingRequest request, SortRequest sort)
        {
            var list = request.Sorts.ToList();
            list.Remove(sort);
            request.Sorts = list;
        }
    }
}
