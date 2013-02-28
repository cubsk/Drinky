using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;

namespace Drinky.MVC.Binders
{
    public class NullableGuidModelBinder :ChainableModelBinder 
    {

        protected override BindAttemptResult AttempGetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
           var propertyType = propertyDescriptor.PropertyType;
           if (propertyType != typeof(Guid?))
               return new BindAttemptResult() { Success = false };

            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if( providerValue == null)
                return new BindAttemptResult() { Success = false };


            var value = providerValue.RawValue;
            if (value == null)
                return new BindAttemptResult() { Success = false };


            var valueType = value.GetType();


            if (valueType == typeof(string[]))
            {
                var attemptedValue = ((string[])value)[0];
                if (string.IsNullOrEmpty(attemptedValue) || string.Compare(attemptedValue, "none", true) == 0)
                return new BindAttemptResult() { Success = true, Value = null};
            }
            else if (valueType == typeof(string))
            {
                var attemptedValue = (string)value;
                if (string.IsNullOrEmpty(attemptedValue) || string.Compare(attemptedValue, "none", true) == 0)
                    return new BindAttemptResult() { Success = true, Value = null };

            }

            return new BindAttemptResult() { Success = false };



        }

    }
}
