﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using NovelWorld.Domain.Exceptions;

namespace NovelWorld.API.Filters
{
    public class RequestValidationFilter : IActionFilter, IOrderedFilter
    {
        // Run first
        public int Order => 1;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            OnActionExecutingInternal(context);
        }

        internal static void OnActionExecutingInternal(ActionExecutingContext context)
        {
            if(context.Result == null && !context.ModelState.IsValid)
            {
                IEnumerable<ValidationFailure> errors = new List<ValidationFailure>();

                foreach(var prop in context.ModelState.ToDictionary(x=>x.Key, x => x.Value))
                {
                    errors = errors.Concat(prop.Value.Errors.Select(x => new ValidationFailure(prop.Key, x.ErrorMessage)));
                }

                if (errors.Any())
                {
                    throw new BadRequestException(errors);
                }
            }
        }
    }

    public class RequestValidationFilterAttribute : ActionFilterAttribute
    {
        public RequestValidationFilterAttribute()
        {
            Order = 1;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RequestValidationFilter.OnActionExecutingInternal(context);
        }
    }
}
