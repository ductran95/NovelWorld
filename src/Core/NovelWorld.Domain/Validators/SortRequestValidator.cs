using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;

namespace NovelWorld.Domain.Validators
{
    public class SortRequestValidator<T>: PropertyValidator
    {
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue is SortRequest sort)
            {
                var propNames = typeof(T).GetProperties(DefaultValues.SearchPropertyFlags).Select(x => x.Name);
                var prop = propNames.FirstOrDefault(x => x == sort.Field);
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

    public static class SortRequestValidatorExtensions
    {
        public static IRuleBuilder<T, SortRequest> ValidateField<T>(this IRuleBuilder<T, SortRequest> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SortRequestValidator<T>());
        }
    }
}