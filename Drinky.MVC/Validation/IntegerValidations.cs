using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Drinky.MVC.Validation
{
    public static class IntegerValidations
    {
        public static bool RequireNumericValue<T>(this Validator<T> validator, Expression<Func<T, double>> property, double? lowerBound, double? upperBound, string errorMessage)
            where T: class
        {
            double? value = property.Compile().Invoke(validator.Model);
            if (!value.HasValue || ( upperBound.HasValue && value.Value >= upperBound) || (lowerBound.HasValue && value.Value <= lowerBound))
            {
                validator.AddError(property, errorMessage);
                return false;
            }
            return true;

        }

        public static bool RequireNumericValue<T>(this Validator<T> validator, Expression<Func<T, double?>> property, double? lowerBound, double? upperBound, string errorMessage)
            where T : class
        {
            double? value = property.Compile().Invoke(validator.Model);
            if (!value.HasValue || (upperBound.HasValue && value.Value >= upperBound) || (lowerBound.HasValue && value.Value <= lowerBound))
            {
                validator.AddError(property, errorMessage);
                return false;
            }
            return true;

        }
    }
}
