using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;

namespace NovelWorld.Domain.Validators
{
    public class FilterRequestValidator<T>: PropertyValidator
    {
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue is FilterRequest filter)
            {
                var propNames = typeof(T).GetProperties(DefaultValues.SearchPropertyFlags).Select(x => x.Name);
                var prop = propNames.FirstOrDefault(x => x == filter.Field);
                if (prop == null)
                {
                    context.MessageFormatter.AppendArgument("TypeName", typeof(T).Name);
                    return false;
                }
            }

            return true;
        }

        protected override string GetDefaultMessageTemplate()
        {
            return "{PropertyName} Field is not in type {TypeName}";
        }
    }

    public static class FilterRequestValidatorExtensions
    {
        public static IRuleBuilder<T, FilterRequest> ValidateField<T>(this IRuleBuilder<T, FilterRequest> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new FilterRequestValidator<T>());
        }
    }
}