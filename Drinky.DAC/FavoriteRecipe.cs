using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;


namespace Drinky.DAC
{
    public class FavoriteRecipe
    {
        public virtual Guid Id { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual UserInfo User { get; set; }

    }

    public class FavoriteRecipeMapping : ClassMap<FavoriteRecipe>
    {

        public FavoriteRecipeMapping()
        {
            Table("FavoriteRecipe");
            Id(x => x.Id, "Id");
            References(x => x.Recipe, "RecipeId");
            References(x => x.User, "UserId");
        }

    }
}