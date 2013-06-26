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
        public IEnumerable<RecipeListItemModel> Get()
        {
            using (var transaction = SessionManager.BeginTransaction())
            {
                return transaction.Session.Query<Recipe>().OrderBy(x => x.Name).Select(x => new RecipeListItemModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsFavorite = x.Favorites.Any(f => f.User.UserName == User.Identity.Name)
                
                }).ToList();

                
            }

        }
    }
}
