using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;


namespace Drinky.MVC
{
    public class Validator<T>
        where T: class
    {
        public string RootPath { get; set; }
        public T Model { get; set; }
        public ModelStateDictionary ModelState { get; set; }

        public Validator(T model, ModelStateDictionary modelState, string rootPath)
            : this(model, modelState)
        {
            this.RootPath = rootPath;
        }

        public Validator(T model, ModelStateDictionary modelState)
        {
            this.Model = model;
            this.ModelState = modelState;
        }

        public void AddError<P>(Expression<Func<T, P>> property, string errorMessage)
        {
            string propertyKey = GetPropertyKey(property);
            ModelState.AddModelError(propertyKey, errorMessage);
        }

        public void AddError(string propertyKey, string errorMessage)
        {
            ModelState.AddModelError(propertyKey, errorMessage);

        }

        protected string GetPropertyKey< P>(Expression<Func<T, P>> property)
        {
            string propertyKey = ExpressionHelper.GetExpressionText(property);
            if (!string.IsNullOrWhiteSpace(RootPath))
                return string.Format("{0}.{1}", RootPath, propertyKey);
            else
                return propertyKey;
        }

        protected string GetPropertyValue<P>(Expression<Func<T, P>> property)
        {
            string propertyKey = ExpressionHelper.GetExpressionText(property);
            if (!string.IsNullOrWhiteSpace(RootPath))
                return string.Format("{0}.{1}", RootPath, propertyKey);
            else
                return propertyKey;
        }

        public bool RequireValue(Expression<Func<T, string>> property, string errorMessage)
        {
            string value = property.Compile().Invoke(Model);

            if (string.IsNullOrWhiteSpace(value) || string.Compare(value, "none", true) == 0)
            {
                AddError(property, errorMessage);
                return false;
            }
            return true;
        }


    }
}
