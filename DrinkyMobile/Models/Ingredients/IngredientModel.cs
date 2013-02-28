using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drinky.MVC;

namespace DrinkyMobile.Models.Ingredients
{
    public class IngredientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string IngredientType { get; set; }
        public List<ListEntry> IngredientTypes { get; set; }


    }
}