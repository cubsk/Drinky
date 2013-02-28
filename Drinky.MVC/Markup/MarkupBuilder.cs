using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Drinky.MVC.Markup
{
    public class MarkupBuilder : TagBuilder
    {
        protected List<MarkupBuilder> PreChildren { get; set; }
        protected List<MarkupBuilder> PostChildren { get; set; }

        public MarkupBuilder(string tagName)
            : base(tagName)
        {
            this.PostChildren = new List<MarkupBuilder>();
            this.PreChildren = new List<MarkupBuilder>();
        }

        public void PrependChild(MarkupBuilder childControl)
        {
            this.PreChildren.Add(childControl);
        }

        public void AddChild(MarkupBuilder childControl)
        {
            this.PostChildren.Add(childControl);
        }

        public new string ToString(TagRenderMode renderMode)
        {
            switch (renderMode)
            {
                case TagRenderMode.SelfClosing:
                    return base.ToString(TagRenderMode.SelfClosing);
                case TagRenderMode.StartTag:
                    return base.ToString(TagRenderMode.StartTag);
                case TagRenderMode.EndTag:
                    return base.ToString(TagRenderMode.EndTag);
                case TagRenderMode.Normal:
                    StringBuilder markup = new StringBuilder();
                    markup.AppendLine(base.ToString(TagRenderMode.StartTag));
                    foreach (MarkupBuilder childTag in PreChildren)
                        markup.AppendLine(childTag.ToString(TagRenderMode.Normal));

                    if (!string.IsNullOrWhiteSpace(base.InnerHtml))
                        markup.AppendLine(base.InnerHtml);

                    foreach (MarkupBuilder childTag in PostChildren)
                        markup.AppendLine(childTag.ToString(TagRenderMode.Normal));

                    markup.AppendLine(base.ToString(TagRenderMode.EndTag));
                    return markup.ToString();
                default:
                    throw new ArgumentException("Unknown tag render mode");
            }
        }

        public override string ToString()
        {
            return ToString(TagRenderMode.Normal);
        }

        public MarkupBuilder CreateChildTag(string tagName)
        {
            MarkupBuilder childControl = new MarkupBuilder(tagName);
            AddChild(childControl);
            return childControl;
        }

        public MarkupBuilder CreatePrependedChildTag(string tagName)
        {
            MarkupBuilder childControl = new MarkupBuilder(tagName);
            PrependChild(childControl);
            return childControl;
        }

        public void ApplyHtmlProperties(object attributes)
        {
            if (attributes == null)
                return;

            foreach (var attr in attributes.GetType().GetProperties())
            {
                string attrName = attr.Name.ToLower();
                string attrVal = attr.GetValue(attributes, new object[] { }).ToString();
                if (attrName == "class")
                {
                    if (Attributes.ContainsKey("class"))
                    {
                        Attributes["class"] = Attributes["class"] + " " + attrVal;
                    }
                    else
                    {
                        Attributes["class"] = attrVal;
                    }

                }
                else
                {
                    Attributes[attrName] = attrVal;

                }
            }

        }

        public bool HasClass(string className)
        {
            if (!Attributes.ContainsKey("class"))
                return false;
            List<string> classes = Attributes["class"].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return classes.Contains(className);

        }

        public string this[string attribute]
        {
            get
            {
                return Attributes[attribute];
            }
            set
            {
                Attributes[attribute] = value;
            }

        }

        public void ApplyAttributes(object attributes)
        {
            if (attributes != null)
            {
                Type attributeType = attributes.GetType();

                foreach (var prop in attributeType.GetProperties())
                {
                    string propName = prop.Name.Replace('_', '-');
                    object value = prop.GetValue(attributes, null);
                    if (value != null)
                        this[propName] = value.ToString();
                    else
                        this[propName] = "";
                }
            }
        }
    }
}
