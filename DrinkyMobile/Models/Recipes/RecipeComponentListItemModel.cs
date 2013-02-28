    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.Recipes
{
    public class RecipeComponentListItemModel
    {
        public Guid? ComponentId { get; set; }
        public Guid? IngredientId { get; set; }
        public Guid? UnitOfMeasureId { get; set; }
        public double? Quantity { get; set; }
        public string Description {get;set;}
        public string EditURL { get; set; }
        public int BaseSortOrder { get; set; }
    }
}