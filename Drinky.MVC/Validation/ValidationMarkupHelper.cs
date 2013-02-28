using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using Drinky.MVC.Markup;

namespace Drinky.MVC
{
    public static class MVCValidationHelper
    {
        public static MvcHtmlString LabelWithValidationFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel, TProperty>> property, string label)
        {
            //<label for="Name" class="ui-input-text">Name:</label>
            string propertyName = ExpressionHelper.GetExpressionText(property);
            var labelTag = new MarkupBuilder("label");
            labelTag["for"] = propertyName;
            labelTag.InnerHtml = label;

            if (htmlHelper.ViewData.ModelState.HasErrorFor(propertyName))
            {
                labelTag.AddCssClass("ui-validation-error");
            }
            return new MvcHtmlString(labelTag.ToString());
        }


        public static bool HasErrorFor(this ModelStateDictionary modelState, string propertyName)
        {
            if (modelState.ContainsKey(propertyName) && modelState[propertyName].Errors.Count > 0)
                return true;
            else 
                return false;
        }
    }
}
