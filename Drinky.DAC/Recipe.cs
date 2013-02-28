using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Drinky.DAC
{

    public class Recipe
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Preparation { get; set; }
        public virtual Glassware Glassware { get; set; }
        public virtual IList<RecipeComponent> Components { get; set; }
        public virtual IList<FavoriteRecipe> Favorites { get; set; }
    }

    public class RecipeMapping : ClassMap<Recipe>
    {

        public RecipeMapping()
        {
            Table("Recipe");
            Id(x => x.Id, "Id");
            Map(x => x.Name, "Name");
            Map(x => x.Preparation, "Preparation");
            References(x => x.Glassware, "GlasswareId");
            HasMany(x => x.Components).KeyColumn("RecipeId").Inverse().Cascade.DeleteOrphan();
            HasMany(x => x.Favorites).KeyColumn("RecipeId").Inverse().Cascade.DeleteOrphan();
        }

    }
}
