using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;

namespace Drinky.MVC.Binders
{
    public class EnumConverterModelBinder :ChainableModelBinder 
    {

        protected override BindAttemptResult AttempGetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
           var propertyType = propertyDescriptor.PropertyType;
           if (!propertyType.IsEnum)
               return new BindAttemptResult() { Success = false };

            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if( providerValue == null)
                return new BindAttemptResult() { Success = false };


            var value = providerValue.RawValue;
            if (value == null)
                return new BindAttemptResult() { Success = false };


            var valueType = value.GetType();
            if(valueType.IsEnum)
                return new BindAttemptResult() { Success = false };

            if (valueType == typeof(string[]))
            {
                return new BindAttemptResult() { Success = true, Value = Enum.ToObject(propertyDescriptor.PropertyType, Convert.ToInt32(((string[])value)[0])) };

            }
            else if (valueType == typeof(string))
            {
                return new BindAttemptResult() { Success = true, Value = Enum.ToObject(propertyDescriptor.PropertyType, Convert.ToInt32((string)value)) };

            }
            else
            {
                return new BindAttemptResult() { Success = true, Value = Enum.ToObject(propertyDescriptor.PropertyType, value) };
            }



        }

    }
}
