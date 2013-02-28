using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{
    public class Ingredient
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IngredientType Type { get; set; }
        public virtual string Description { get; set; }

    }


    public class IngredientMapping : ClassMap<Ingredient>
    {

        public IngredientMapping()
        {
            Table("Ingredient");
            Id(x => x.Id, "Id");
            Map(x => x.Name, "Name");
            Map(x => x.Description, "Description");
            References(x => x.Type, "IngredientTypeId");
        }

    }
}
