using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Drinky.MVC.Binders
{
    public class DictionaryModelBinder : ChainableModelBinder
    {

        protected override BindAttemptResult AttempGetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var propertyType = propertyDescriptor.PropertyType;
            if(!propertyType.IsGenericType)
                return new BindAttemptResult() { Success = false };

            // see if our property is a  implementation of a generic dictionary
            var genericTypeDef = propertyType.GetGenericTypeDefinition();
            if (genericTypeDef != typeof(Dictionary<,>))
                return new BindAttemptResult() { Success = false };

            var valueType = propertyType.GetGenericArguments().Skip(1).FirstOrDefault();

            // create a new dictionary object if not already created
            object value = bindingContext.Model ?? Activator.CreateInstance(propertyType);

            // temporary object to hold the dictionary keys
            List<string> indexes = new List<string>();

            // get all of our keys and find the keys that represent indexed values for this dictionary
            var keys = controllerContext.RequestContext.HttpContext.Request.Params.AllKeys;
            var subIndexPattern = new Regex("^" + bindingContext.ModelName + @"\[(?<index>[^\]]+)\](?<prop>.*)"); // "[" then one or more not "]", then "]"
            
            // find the "add" method on our generic dictionary, so we can set the values
            var addMethod = propertyType.GetMethod("Add");

            // foreach key in the request params, find out if it belongs to our dictionary. keep track of keys and bind each value child object
            foreach (var key in keys)
            {
                var match = subIndexPattern.Match(key);
                if (match.Success)
                {
                    var index = match.Groups["index"].Value;
                    var prop = match.Groups["prop"].Value;
                    var prefix = bindingContext.ModelName + "[" + index + "]";

                    if (!indexes.Contains(index))
                    {
                        indexes.Add(index);
                        var subModel = CreateModel(controllerContext, bindingContext, valueType);

                        Func<object> modelAccessor = delegate() { return subModel; };
                        ModelBindingContext subModelContext = new ModelBindingContext(bindingContext)
                        {
                            ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(modelAccessor, valueType),
                            ModelName = prefix,
                            ModelState = bindingContext.ModelState,
                            PropertyFilter = bindingContext.PropertyFilter,
                            ValueProvider = bindingContext.ValueProvider
                        };

                        BindModel(controllerContext, subModelContext);
                        addMethod.Invoke(bindingContext.Model, new object[] { index, subModel });
                    }
                }
            }

           

            return new BindAttemptResult() { Success = true, Value = value };
        }
    }
}
