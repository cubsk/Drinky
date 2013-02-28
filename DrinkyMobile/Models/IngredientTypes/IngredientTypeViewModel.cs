using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkyMobile.Models.IngredientTypes
{
    public class IngredientTypeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int IngredientCount { get; set; }

    }
}