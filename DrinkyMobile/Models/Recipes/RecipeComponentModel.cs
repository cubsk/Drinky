using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drinky.MVC;

namespace DrinkyMobile.Models.Recipes
{
    public class RecipeComponentModel
    {
        public Guid? ComponentId { get; set; }
        public double? Quantity { get; set; }
        public string Description { get; set; }
        public double Increment { get; set; }


        public Guid? IngredientTypeId { get; set; }
        public List<ListEntry> IngredientTypes { get; set; }

        public Guid? IngredientId { get; set; }
        public List<ListEntry> Ingredients { get; set; }

        public Guid? UnitOfMeasureId { get; set; }
        public List<ListEntry> UnitsOfMeasure { get; set; }

        public RecipeComponentModel()
        {
            this.UnitsOfMeasure = new List<ListEntry>();
            this.IngredientTypes = new List<ListEntry>();
        }
    }
}