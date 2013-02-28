using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drinky.MVC
{
    public class ListEntry
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsPlaceholder { get; set; }

        public object  Attributes { get; set; }

        public ListEntry()
        {
            this.IsSelected = true;
            this.IsDisabled = false;
            this.IsPlaceholder = false;
        }

    }
}
