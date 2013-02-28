using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.Recipes
{
    public class RecipeListModel
    {
        public List<RecipeListItemModel> Recipes { get; set; }

        public RecipeListModel()
        {
            Recipes = new List<RecipeListItemModel>();
        }
    }
}