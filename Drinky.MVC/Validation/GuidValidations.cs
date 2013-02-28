using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Drinky.MVC.Validation
{
    public static class GuidValidations
    {
        public static bool RequireGuidValue<T>(this Validator<T> validator, Expression<Func<T, Guid?>> property, string errorMessage)
            where T : class
        {
            Guid? value = property.Compile().Invoke(validator.Model);
            if (!value.HasValue || value.Equals(Guid.Empty))
            {
                validator.AddError(property, errorMessage);
                return false;
            }
            return true;

        }

    }
}
