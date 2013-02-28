using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.IngredientTypes
{
    public class IngredientTypeListModel
    {
        public List<IngredientTypeListItemModel> Items { get; set; }

        public  IngredientTypeListModel()
        {
            Items = new List<IngredientTypeListItemModel>();
        }
    }
}