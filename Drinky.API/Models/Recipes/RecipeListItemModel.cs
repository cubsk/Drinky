using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinky.API.Models.Recipes
{
    public class RecipeListItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
    }
}