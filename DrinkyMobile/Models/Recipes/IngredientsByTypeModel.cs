using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drinky.MVC;

namespace DrinkyMobile.Models.Recipes
{
    public class IngredientsByTypeModel
    {
        public Guid IngredientTypeId { get; set; }
        public Guid IngredientId { get; set; }
        public List<ListEntry> Ingredients { get; set; }

        public IngredientsByTypeModel()
        {
            this.Ingredients = new List<ListEntry>();
        }
    }
}