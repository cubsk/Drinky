using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinky.API.Models.Recipes
{
    public class RecipeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Preperation { get; set; }
        public string GlasswareType { get; set; }

        public List<RecipeViewComponentModel> Components { get; set; }

        public RecipeModel()
        {
            this.Components = new List<RecipeViewComponentModel>();
        }
    }

    public class RecipeViewComponentModel
    {
        public Guid? ComponentId { get; set; }
        public Guid? IngredientTypeId { get; set; }
        public Guid? UnitOfMeasureId { get; set; }
        public Guid? IngredientId { get; set; }

        public string IngredientName { get; set; }
        public string IngredientTypeName { get;set; }

        public double? Quantity { get; set; }
        public string Description { get; set; }
        public double Increment { get; set; }


    }
}