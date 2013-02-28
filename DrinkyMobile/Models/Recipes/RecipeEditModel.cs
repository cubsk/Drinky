using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drinky.MVC;

namespace DrinkyMobile.Models.Recipes
{
    public class RecipeEditModel 
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Preperation { get; set; }

        public Guid GlasswareType { get; set; }
        public List<ListEntry> GlasswareTypes { get; set; }

        public Dictionary<string, RecipeComponentListItemModel> Components { get; set; }

        public RecipeEditModel()
        {
            this.Components = new Dictionary<string, RecipeComponentListItemModel>();
        }

    }
}