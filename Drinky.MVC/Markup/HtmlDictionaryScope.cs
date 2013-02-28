using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Drinky.MVC.Markup
{
    public static class HtmlDictionExtensions
    {
        public static IDisposable BeginDictionaryItemScope(this HtmlHelper html, Guid key)
        {
            return new HtmlDictionaryScope(html.ViewData.TemplateInfo,  "[" + key + "]");
        }

    }

    public  class HtmlDictionaryScope : IDisposable
    {
    
        private readonly TemplateInfo templateInfo;
        private readonly string previousHtmlFieldPrefix;

        public HtmlDictionaryScope(TemplateInfo templateInfo, string htmlFieldPrefix)
        {
            this.templateInfo = templateInfo;

            previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
            templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix + htmlFieldPrefix;
        }

        public void Dispose()
        {
            templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
        }
    }
}
