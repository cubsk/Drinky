using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{
    public class RecipeComponent
    {
        public virtual Guid Id { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public virtual double Quantity { get; set; }

        public static string DescribeComponent(Ingredient ingredient, UnitOfMeasure uom, double quantity)
        {
            string quantityDescription = uom.DescribeQuantity(quantity);
            return string.Format("{0} {1}", quantityDescription, ingredient.Name);

        }

        public virtual string DescribeComponent()
        {
            return DescribeComponent(Ingredient, UnitOfMeasure, Quantity);
        }
    }

    public class RecipeComponentMapping : ClassMap<RecipeComponent>
    {

        public RecipeComponentMapping()
        {
            Table("RecipeComponent");
            Id(x => x.Id, "Id");
            Map(x => x.Quantity, "Quantity");
            References(x => x.Recipe, "RecipeId");
            References(x => x.Ingredient, "IngredientId");
            References(x => x.UnitOfMeasure, "UnitOfMeasureId");

        }

    }
}
