using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.Recipes
{
    public class RecipeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Preperation { get; set; }
        public string GlasswareType { get; set; }

        public List<RecipeViewComponentModel> Components { get; set; }

        public RecipeViewModel()
        {
            this.Components = new List<RecipeViewComponentModel>();
        }
    }

    public class RecipeViewComponentModel
    {
        public string Description {get;set;}
    }
}