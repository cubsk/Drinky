using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Drinky.MVC
{
    public static class ListExtensions
    {
        public static List<ListEntry> WithNoneSelected(this IEnumerable<ListEntry> target)
        {
            List<ListEntry> result = new List<ListEntry>();
            result.Add(new ListEntry() { Text = "None Selected", IsPlaceholder = true});
            result.AddRange(target);
            return result;

        }

        public static List<ListEntry> WithLabel(this IEnumerable<ListEntry> target, string Label)
        {
            List<ListEntry> result = new List<ListEntry>();
            result.Add(new ListEntry() { Text = Label });
            result.AddRange(target);
            return result;
        }

        public static List<ListEntry> WithOption(this IEnumerable<ListEntry> target, string label, string value)
        {
            List<ListEntry> result = new List<ListEntry>();
            result.Add(new ListEntry() { Text = label, Value = value });
            result.AddRange(target);
            return result;
        }
    }
}
