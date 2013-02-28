using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{
    public class IngredientType
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Ingredient> Ingredients { get; set; }
    }

    public class IngredientTypeMapping : ClassMap<IngredientType>
    {

        public IngredientTypeMapping()
        {
            Table("IngredientType");
            Id(x => x.Id, "Id");
            Map(x => x.Name, "Name");
            HasMany(x => x.Ingredients).KeyColumn("IngredientTypeId");
        }

    }
}
