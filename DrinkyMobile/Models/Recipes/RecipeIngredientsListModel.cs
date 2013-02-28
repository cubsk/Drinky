using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.Recipes
{
    public class RecipeIngredientsListModel
    {
        public List<RecipeIngredientsListGroupModel> IngredientGroups { get; set; }

        public RecipeIngredientsListModel()
        {
            this.IngredientGroups = new List<RecipeIngredientsListGroupModel>();
        }
    }

    public class RecipeIngredientsListGroupModel
    {
        public string IngredientName { get; set; }
        public string IngredientTypeName { get; set; }

        public Guid IngredientId { get; set; }
        public List<RecipeIngredientsListItemModel> Recipes { get; set; }

        public RecipeIngredientsListGroupModel()
        {
            this.Recipes = new List<RecipeIngredientsListItemModel>();

        }
    }

    public class RecipeIngredientsListItemModel
    {
        public Guid RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeQuantity { get; set; }

    }
}