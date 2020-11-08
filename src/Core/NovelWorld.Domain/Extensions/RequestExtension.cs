using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Exceptions;

namespace NovelWorld.Domain.Extensions
{
    public static class RequestExtension
    {
        public static void ValidateField<T>(this PagingRequest request)
        {
            var propNames = typeof(T).GetProperties(DefaultValues.SearchPropertyFlags).Select(x => x.Name);
            var errors = new List<ValidationFailure>();

            if (request.Filters != null && request.Filters.Any())
            {
                for (int i = 0; i < request.Filters.Count(); i++)
                {
                    var filter = request.Filters.ElementAt(i);
                    var prop = propNames.FirstOrDefault(x => x.ToLower().Equals(filter.Field.ToLower()));
                    if (prop == null)
                    {
                        errors.Add(new ValidationFailure($"Filters[{i}].Field", CommonErrorMessages.FieldNotFound));
                    }
                    else
                    {
                        filter.Field = prop;
                    }
                }
            }

            if (request.Sorts != null && request.Sorts.Any())
            {
                for (int i = 0; i < request.Sorts.Count(); i++)
                {
                    var sort = request.Sorts.ElementAt(i);
                    var prop = propNames.FirstOrDefault(x => x.ToLower().Equals(sort.Field.ToLower()));
                    if (prop == null)
                    {
                        errors.Add(new ValidationFailure($"Filters[{i}].Field", CommonErrorMessages.FieldNotFound));
                    }
                    else
                    {
                        sort.Field = prop;
                    }
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors, "Validate Paging request fields");
            }
        }

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
