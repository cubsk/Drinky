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
    public static class RadioButtonListHelper
    {

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string label, Expression<Func<TModel, TProperty>> property, IEnumerable<ListEntry> options)
        {
            return RadioButtonListHelper.RadioButtonListFor(htmlHelper, label,property , options, false, false);
        }
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string label, Expression<Func<TModel, TProperty>> property, IEnumerable<ListEntry> options, bool isMini, bool isHorizontal)
        {
            string propertyName = ExpressionHelper.GetExpressionText(property);
            var propertyVal= property.Compile().Invoke(htmlHelper.ViewData.Model);

            string propertyValue = propertyVal != null ? propertyVal.ToString() : "none";

            MarkupBuilder fieldSet = new MarkupBuilder("fieldset");
            fieldSet["data-role"] = "controlgroup";
            if (isMini)
                fieldSet["data-mini"] = "true";
            if (isHorizontal)
                fieldSet["data-type"] = "horizontal";

            var legend = fieldSet.CreateChildTag("legend");
            legend.SetInnerText(label);

            int pos = 1;
            foreach (var option in options)
            {
                string buttonId = string.Format("{0}-{1}", propertyName, pos);
                var button = fieldSet.CreateChildTag("input");
                button["type"] = "radio";
                button["name"] = propertyName;
                button["id"] = buttonId;
                button["value"] = option.Value;
                if (string.Compare(propertyValue, option.Value, true) == 0)
                    button["checked"] = "checked";

                var buttonLabel = fieldSet.CreateChildTag("label");
                buttonLabel["for"] = buttonId;
                buttonLabel.SetInnerText(option.Text);

                button.ApplyAttributes(option.Attributes);

                pos++;
            }

            if (htmlHelper.ViewData.ModelState.HasErrorFor(propertyName))
            {
                legend.AddCssClass("ui-validation-error");
            }

            return new MvcHtmlString(fieldSet.ToString());
        }
    }
}
