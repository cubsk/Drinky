using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;

namespace Drinky.MVC.Binders
{
    public abstract class ChainableModelBinder : DefaultModelBinder
    {
        protected ChainableModelBinder Next { get; set; }

        public ChainableModelBinder Chain(ChainableModelBinder NextBinder)
        {
            this.Next = NextBinder;
            return NextBinder;
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            BindAttemptResult bindAttempt = AttempGetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
            if (bindAttempt.Success)
                return bindAttempt.Value;
            else
            {
                if (Next != null)
                    return Next.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
                else
                    return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
            }
            
        }


        protected abstract BindAttemptResult AttempGetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder);
    }

    public class BindAttemptResult
    {
        public bool Success { get; set; }
        public object Value { get; set; }

    }
}
