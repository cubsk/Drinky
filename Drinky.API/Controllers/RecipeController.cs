using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Drinky.DAC;
using NHibernate.Linq;
using Drinky.API.Models.Recipes;
using NHibernate;
using Drinky.API.Security;

namespace Drinky.API.Controllers
{
    [BasicAuthenticationFilter]
    public class RecipeController : ApiController
    {
        public IEnumerable<RecipeListItemModel> Get(string filter)
        {
            using (var transaction = SessionManager.BeginTransaction())
            {
                var query = transaction.Session.Query<Recipe>();

                if (!string.IsNullOrWhiteSpace(filter))
                    query = query.Where(x => x.Name.Contains(filter));

                return query.Select(x => new RecipeListItemModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsFavorite = x.Favorites.Any(f => f.User.UserName == User.Identity.Name)

                }).OrderBy(x => x.Name).ToList();

                
            }

        }

        public RecipeModel GetRecipeById(Guid Id)
        {
            using (var transaction = SessionManager.BeginTransaction())
            {
                RecipeModel model = new RecipeModel();

                Recipe r = transaction.Session.Get<Recipe>(Id);
                model.Id = r.Id;
                model.Name = r.Name;
                model.Preperation = r.Preparation;
                model.GlasswareType = r.Glassware.Name;

                foreach (var component in r.Components.OrderBy(x => x.UnitOfMeasure.SortOrder).ThenByDescending(x => x.Quantity))
                {
                    RecipeViewComponentModel componentLineModel = new RecipeViewComponentModel();
                    componentLineModel.Description = component.DescribeComponent();
                    componentLineModel.ComponentId = component.Id;
                    componentLineModel.Quantity = component.Quantity;

                    componentLineModel.IngredientId = component.Ingredient.Id;
                    componentLineModel.IngredientName = component.Ingredient.Name;

                    componentLineModel.IngredientTypeId = component.Ingredient.Type.Id;
                    componentLineModel.IngredientTypeName = component.Ingredient.Type.Name;

                    model.Components.Add(componentLineModel);
                }

                return model;

            }

        }
    }
}
