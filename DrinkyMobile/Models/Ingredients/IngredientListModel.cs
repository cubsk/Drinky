using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.Ingredients
{
    public class IngredientListModel
    {
        public List<IngredientListItemTypeGroupModel> IngredientGroups { get; set; }

        public IngredientListModel()
        {
            IngredientGroups = new List<IngredientListItemTypeGroupModel>();

        }
    }

    public class IngredientListItemTypeGroupModel
    {
        public Guid TypeId { get; set; }
        public string TypeName { get; set; }

        public List<IngredientListItemModel> Ingredients { get; set; }

        public IngredientListItemTypeGroupModel()
        {
            this.Ingredients = new List<IngredientListItemModel>();
        } 

    }
}