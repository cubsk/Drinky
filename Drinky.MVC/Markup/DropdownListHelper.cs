using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Reflection;

namespace Drinky.MVC.Markup
{
    public static class DropDownListHelper
    {
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property, IEnumerable<ListEntry> options)
        {
            return DropDownListHelper.DropDownListFor(htmlHelper, property, options, false);
        }
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property, IEnumerable<ListEntry> options, bool isMini)
        {
            string propertyName = ExpressionHelper.GetExpressionText(property);
            var propertyVal = property.Compile().Invoke(htmlHelper.ViewData.Model);

            string propertyValue = propertyVal != null ? propertyVal.ToString() : "none";

            MarkupBuilder selectTag = new MarkupBuilder("select");
            selectTag["name"] = propertyName;
            selectTag["id"] = propertyName;
            if (isMini)
                selectTag["data-mini"] = "true";


            foreach (var option in options)
            {
                var optTag = selectTag.CreateChildTag("option");
                optTag.InnerHtml = option.Text;
                if (!string.IsNullOrEmpty(option.Value))
                    optTag.Attributes["value"] = option.Value;
                optTag.ApplyAttributes(option.Attributes);

                if (string.Compare(propertyValue, option.Value, true) == 0)
                    optTag["selected"] = "selected";

                if(option.IsDisabled)
                    optTag["disabled"] = "selected";

                if (option.IsPlaceholder)
                    optTag["data-placeholder"] = "true";
            }


            return new MvcHtmlString(selectTag.ToString());
        }
    }

}
