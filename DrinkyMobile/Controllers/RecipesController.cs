using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drinky.DAC;
using NHibernate.Linq;
using Drinky.MVC;
using Drinky.MVC.Validation;
using Drinky.MVC.Markup;
using DrinkyMobile.Models.Recipes;
using DrinkyMobile.Models.Common;
using System.Web.Security;

namespace DrinkyMobile.Controllers
{
    public class RecipesController : Controller
    {

        public ActionResult Edit(Guid? Id)
        {
            var session = SessionManager.GetSession();
         
            RecipeEditModel model = new RecipeEditModel();
            model.GlasswareTypes = GetGlasswareTypes();

            if (Id.HasValue)
            {
                Recipe recipe = session.Get<Recipe>(Id.Value);
                model.Name = recipe.Name;
                model.Preperation = recipe.Preparation;
                model.GlasswareType = recipe.Glassware.Id;

                foreach (var ingredient in recipe.Components)
                {
                    RecipeComponentListItemModel component = new RecipeComponentListItemModel();


                    component.ComponentId = ingredient.Id;
                    component.IngredientId = ingredient.Ingredient.Id;
                    component.Quantity = ingredient.Quantity;
                    component.UnitOfMeasureId = ingredient.UnitOfMeasure.Id;
                    component.Description = ingredient.DescribeComponent();
                    component.EditURL = Url.Action("EditComponent", "Recipes", component);
                    component.BaseSortOrder = ingredient.UnitOfMeasure.SortOrder;

                    model.Components.Add(ingredient.Id.ToString(),component);

                }
            }


            ViewData.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(RecipeEditModel model)
        {
            var session = SessionManager.GetSession();

            model.GlasswareTypes = GetGlasswareTypes();

            if (ValidateRecipeEdit(model))
            {
                using (var transaction = SessionManager.BeginTransaction())
                {
                    if (model.Id.HasValue)
                    {
                        UpdateExistingRecipe(model);
                    }
                    else
                    {
                        CreateNewRecipe(model);
                    }

                    transaction.Commit();
                    return Redirect("/Recipes/List/");

                }
            }

            // restore description
            foreach (var component in model.Components.Values)
            {
                UnitOfMeasure uom = session.Get<UnitOfMeasure>(component.UnitOfMeasureId.Value);
                Ingredient ingredient = session.Get<Ingredient>(component.IngredientId.Value);

                component.Description = RecipeComponent.DescribeComponent(ingredient, uom, component.Quantity.Value);
                component.EditURL = Url.Action("EditComponent", "Recipes", component);
            }

            ViewData.Model = model;
            return View();
        }


        [NonAction]
        public Recipe CreateNewRecipe(RecipeEditModel model)
        {
            var session = SessionManager.GetSession();


            Recipe newRecipe = new Recipe();
            newRecipe.Name = model.Name;
            newRecipe.Preparation = model.Preperation;
            newRecipe.Glassware = new Glassware() { Id = model.GlasswareType };

            session.SaveOrUpdate(newRecipe);

            foreach (var ingredient in model.Components.Values)
            {
                RecipeComponent component = new RecipeComponent();
                component.Recipe = newRecipe;
                component.UnitOfMeasure = new UnitOfMeasure() { Id = ingredient.UnitOfMeasureId.Value };
                component.Ingredient = new Ingredient() { Id = ingredient.IngredientId.Value };
                component.Quantity = ingredient.Quantity.Value;

                session.SaveOrUpdate(component);
            }
            return newRecipe;
        }

        [NonAction]
        public Recipe UpdateExistingRecipe(RecipeEditModel model)
        {
            var session = SessionManager.GetSession();


            Recipe existingRecipe = session.Get<Recipe>(model.Id);
            existingRecipe.Name = model.Name;
            existingRecipe.Preparation = model.Preperation;
            existingRecipe.Glassware = new Glassware() { Id = model.GlasswareType };

            session.SaveOrUpdate(existingRecipe);

            // update existing components or add new ones
            foreach (var ingredient in model.Components.Values)
            {
                var existingIngredient = existingRecipe.Components.Where(x => x.Id == ingredient.ComponentId.Value).SingleOrDefault();
                if (existingIngredient == null)
                {
                    existingIngredient = new RecipeComponent() { Recipe = existingRecipe };
                }

                existingIngredient.UnitOfMeasure = new UnitOfMeasure() { Id = ingredient.UnitOfMeasureId.Value };
                existingIngredient.Ingredient = new Ingredient() { Id = ingredient.IngredientId.Value };
                existingIngredient.Quantity = ingredient.Quantity.Value;

                session.SaveOrUpdate(existingIngredient);
            }

            foreach (var component in existingRecipe.Components)
            {
                if (!model.Components.Values.Any(c => c.ComponentId.Value == component.Id))
                {
                    session.Delete(component);
                }
            }
            return existingRecipe;
        }

        [NonAction]
        protected bool ValidateRecipeEdit(RecipeEditModel model)
        {
            var session = SessionManager.GetSession();

            var validator = new Validator<RecipeEditModel>(model, ViewData.ModelState);
            validator.RequireValue(x => x.Name, "Please provide a name ");
            validator.RequireValue(x => x.Preperation, "Please specify a method of preparation");
            validator.RequireGuidValue(x => x.GlasswareType, "Please select an glassware type");
            
            if (model.Components.Count < 1)
                validator.AddError("components", "Please select at least one ingredient");

            if (model.Id.HasValue)
            {
                if (session.Query<Recipe>().Any(x => x.Name == model.Name && x.Id != model.Id.Value))
                    validator.AddError(x => x.Name, "A recipe with this name already exists");
            }
            else
            {
                if (session.Query<Recipe>().Any(x => x.Name == model.Name ))
                    validator.AddError(x => x.Name, "A recipe with this name already exists");
            }
            return ViewData.ModelState.IsValid;
        }



        public ActionResult View(Guid Id)
        {
            var session = SessionManager.GetSession();
            RecipeViewModel model = new RecipeViewModel();

            Recipe r = session.Get<Recipe>(Id);
            model.Id = r.Id;
            model.Name = r.Name;
            model.Preperation = r.Preparation;
            model.GlasswareType = r.Glassware.Name;

            foreach (var ingredient in r.Components.OrderBy(x=> x.UnitOfMeasure.SortOrder).ThenByDescending(x=> x.Quantity))
            {
                RecipeViewComponentModel componentLineModel = new RecipeViewComponentModel();
                componentLineModel.Description = ingredient.DescribeComponent();
                model.Components.Add(componentLineModel);
            }

            
            ViewData.Model = model;
            return View();
        }

        public ActionResult List()
        {
           
            RecipeListModel model = new RecipeListModel();

            var session = SessionManager.GetSession();

            model.Recipes = session.Query<Recipe>().OrderBy(x => x.Name).Select(x => new RecipeListItemModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsFavorite = x.Favorites.Any(f => f.User.UserName == User.Identity.Name)
                
            }).ToList();

            ViewData.Model = model;
            return View();
        }

        public ActionResult ListFavorites()
        {

            RecipeListModel model = new RecipeListModel();

            var session = SessionManager.GetSession();

            model.Recipes = session.Query<Recipe>().OrderBy(x => x.Name)
            .Where(x => x.Favorites.Any(f => f.User.UserName == User.Identity.Name))
            .Select(x => new RecipeListItemModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsFavorite = true

            }).ToList();

            ViewData.Model = model;
            return View("List");
        }

        public ActionResult ListByIngredient()
        {
            var session = SessionManager.GetSession();

            var recipesGroupedByIngredients = session.Query<RecipeComponent>()
                .Select(x => new
                {
                    IngredientTypeName = x.Ingredient.Type.Name,
                    IngredientId = x.Ingredient.Id,
                    IngredientName = x.Ingredient.Name,
                    RecipeId = x.Recipe.Id,
                    RecipeName = x.Recipe.Name,
                    ComponentQuanity = x.Quantity,
                    ComponentUnitOfMeasureAbbreviation = x.UnitOfMeasure.Abbreviation
                })
                .ToList()
                .GroupBy(x => new
                {
                    IngredientId = x.IngredientId,
                    IngredientName = x.IngredientName,
                    IngredientTypeName = x.IngredientTypeName
                })
                .OrderBy(x => x.Key.IngredientTypeName).ThenBy( x=>  x.Key.IngredientName )
                .ToList();

            var model = new RecipeIngredientsListModel();
            foreach (var recipeGrouping in recipesGroupedByIngredients)
            {
                var ingredientGrouping = new RecipeIngredientsListGroupModel();
                model.IngredientGroups.Add(ingredientGrouping);
                ingredientGrouping.IngredientId = recipeGrouping.Key.IngredientId;
                ingredientGrouping.IngredientName = recipeGrouping.Key.IngredientName;
                ingredientGrouping.IngredientTypeName = recipeGrouping.Key.IngredientTypeName;

                foreach(var recipe in recipeGrouping.OrderBy(x=> x.RecipeName))
                {
                    var recipeModel = new RecipeIngredientsListItemModel();
                    ingredientGrouping.Recipes.Add(recipeModel);
                    recipeModel.RecipeId = recipe.RecipeId;
                    recipeModel.RecipeName = recipe.RecipeName;
                    recipeModel.RecipeQuantity = UnitOfMeasure.DescribeQuantity(recipe.ComponentQuanity, recipe.ComponentUnitOfMeasureAbbreviation);
                }
            }

            ViewData.Model = model;
            return View();
        }

        public ActionResult ConfirmDelete(Guid Id)
        {
            RecipeEditModel model = new RecipeEditModel();
            model.Id = Id;
            ViewData.Model = model;
            return View();
        }

        public ActionResult Delete(Guid Id)
        {
            using (NHTransaction transaction = SessionManager.BeginTransaction())
            {
                var recipe = transaction.Session.Get<Recipe>(Id);
                transaction.Session.Delete(recipe);
                transaction.Commit();
                return View();
            }
        }

        public ActionResult AddComponent()
        {
            var session = SessionManager.GetSession();
            RecipeComponentModel model = new RecipeComponentModel();

            // set the default increment from the unit of measure
            var uom = session.Query<UnitOfMeasure>().Where(x => x.IsDefault == true).Single();
            model.UnitOfMeasureId = uom.Id;
            model.Increment = uom.DefaultIncrement;

            // set default quantity
            model.Quantity = 1;

            // if ingredient id specified, override ingredient type id to match
            if (model.IngredientId.HasValue)
            {
                var ingredient = session.Get<Ingredient>(model.IngredientId.Value);
                model.IngredientTypeId = ingredient.Type.Id;
            }


            // fill out our drop downs
            model.UnitsOfMeasure = GetUnitsOfMeasure();
            model.IngredientTypes = GetIngredientTypes();
            model.Ingredients = GetIngredients(model.IngredientId);

            ViewData.Model = model;
            return PartialView("EditComponent");

        }

        public ActionResult EditComponent(RecipeComponentListItemModel existingComponent)
        {
            var session = SessionManager.GetSession();

            RecipeComponentModel model = new RecipeComponentModel();

            model.ComponentId = existingComponent.ComponentId;
            model.IngredientId = existingComponent.IngredientId;
            model.Quantity = existingComponent.Quantity;
            model.UnitOfMeasureId = existingComponent.UnitOfMeasureId;

            var uom = session.Get<UnitOfMeasure>(existingComponent.UnitOfMeasureId.Value);
            model.Increment = uom.DefaultIncrement;


            // if ingredient id specified, override ingredient type id to match
            if (model.IngredientId.HasValue)
            {
                var ingredient = session.Get<Ingredient>(model.IngredientId.Value);
                model.IngredientTypeId = ingredient.Type.Id;
            }


            // fill out our drop downs
            model.UnitsOfMeasure = GetUnitsOfMeasure();
            model.IngredientTypes = GetIngredientTypes();
            model.Ingredients = GetIngredients(model.IngredientTypeId);

            ViewData.Model = model;

            return View();
        }


        public ActionResult MakeFavorite(Guid RecipeId)
        {
            using (NHTransaction transaction = SessionManager.BeginTransaction())
            {
                UserInfo user = transaction.Session.Query<UserInfo>().Where(x => x.UserName == User.Identity.Name).SingleOrDefault();
                if (user == null) throw new Exception("User is not authenticated");

                Recipe r = transaction.Session.Get<Recipe>(RecipeId);
                if (r == null) throw new ArgumentException("Unknown recipe specified");

                var favorite = transaction.Session.Query<FavoriteRecipe>().Where(x => x.User.Id == user.Id && x.Recipe.Id == RecipeId).SingleOrDefault();
                if (favorite == null)
                {
                    favorite = new FavoriteRecipe();
                    favorite.Recipe = r;
                    favorite.User = user;

                    transaction.Session.SaveOrUpdate(favorite);
                }

                transaction.Commit();

            }


            return Json(new 
            {
                Success = true,
                Message = "Recipe marked as favorite"

            });

        }

        public ActionResult RemoveFromFavorites(Guid RecipeId)
        {
            using (NHTransaction transaction = SessionManager.BeginTransaction())
            {
                UserInfo user = transaction.Session.Query<UserInfo>().Where(x => x.UserName == User.Identity.Name).SingleOrDefault();
                if (user == null) throw new Exception("User is not authenticated");

                Recipe r = transaction.Session.Get<Recipe>(RecipeId);
                if (r == null) throw new ArgumentException("Unknown recipe specified");

                var favorite = transaction.Session.Query<FavoriteRecipe>().Where(x => x.User.Id == user.Id && x.Recipe.Id == RecipeId).SingleOrDefault();
                if (favorite != null)
                {
                    transaction.Session.Delete(favorite);
                }
                transaction.Commit();
            }

            return Json(new
            {
                Success = true,
                Message = "Recipe removed from favorites"

            });

        }


        [HttpPost]
        public ActionResult EditComponent(RecipeComponentModel model)
        {
   
            var session = SessionManager.GetSession();

            model.UnitsOfMeasure = GetUnitsOfMeasure();
            model.IngredientTypes = GetIngredientTypes();
            model.Ingredients = GetIngredients(model.IngredientTypeId);



            if (model.UnitOfMeasureId.HasValue)
            {
                var uom = session.Get<UnitOfMeasure>(model.UnitOfMeasureId.Value);
                model.Increment = uom.DefaultIncrement;
            }

            if (ValidateComponentEdit(model))
            {
                RecipeComponentListItemModel listItemModel = new RecipeComponentListItemModel();
                listItemModel.ComponentId = model.ComponentId ?? Guid.NewGuid();
                listItemModel.IngredientId = model.IngredientId;
                listItemModel.Quantity = model.Quantity;
                listItemModel.UnitOfMeasureId = model.UnitOfMeasureId;

                UnitOfMeasure uom = session.Get<UnitOfMeasure>(listItemModel.UnitOfMeasureId.Value);
                Ingredient ingredient = session.Get<Ingredient>(model.IngredientId.Value);
                listItemModel.BaseSortOrder = uom.SortOrder;
                listItemModel.Description = RecipeComponent.DescribeComponent(ingredient, uom, listItemModel.Quantity.Value);

                listItemModel.EditURL = Url.Action("EditComponent", "Recipes", listItemModel);

                ViewData.TemplateInfo.HtmlFieldPrefix = "Components";
                return PartialView("RecipeComponentListItem",listItemModel);
            }
            else
            {
                return PartialView(model);

            }

        }


        public ActionResult ComponentListItem(RecipeComponentListItemModel component)
        {
            ViewData.Model = component;
            return View("RecipeComponentListItem");
        }

        public ActionResult IngredientsByType(Guid? IngredientTypeId)
        {
            IngredientsByTypeModel model = new IngredientsByTypeModel();
            model.Ingredients = GetIngredients(IngredientTypeId);
            ViewData.Model = model;
            return PartialView();
        }

        [NonAction]
        protected bool ValidateComponentEdit(RecipeComponentModel model)
        {
            var validator = new Validator<RecipeComponentModel>(model, ViewData.ModelState);
            validator.RequireNumericValue(x => x.Quantity, 0, null, "Quantity must be greater than zero");
            validator.RequireGuidValue(x => x.IngredientId, "Please select an ingredient");
            validator.RequireGuidValue(x => x.UnitOfMeasureId, "Please select a unit of measure");
            return ViewData.ModelState.IsValid;
        }


        [NonAction]
        protected List<ListEntry> GetGlasswareTypes()
        {
            return SessionManager.GetSession().Query<Glassware>().OrderBy(x => x.Name).Select(x => new ListEntry()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

        }

        [NonAction]
        protected List<ListEntry> GetIngredientTypes()
        {
            return SessionManager.GetSession().Query<IngredientType>()
                .Where(x=> x.Ingredients.Count > 0)
                .Select(x => new ListEntry()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text).WithOption("Ingredient Type", "none").ToList();

        }

        [NonAction]
        protected List<ListEntry> GetIngredients(Guid? ingredientTypeId)
        {
            var query = SessionManager.GetSession().Query<Ingredient>();
            if(ingredientTypeId.HasValue)
                query = query.Where(x => x.Type.Id == ingredientTypeId.Value);

            return query.Select(x => new ListEntry()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Attributes = new  { 
                    data_ingredienttypeid = x.Type.Id.ToString() 
                }

            }).OrderBy(x => x.Text).WithOption("Ingredient", "none").ToList();

        }

        [NonAction]
        protected List<ListEntry> GetUnitsOfMeasure()
        {
            return SessionManager.GetSession().Query<UnitOfMeasure>().OrderBy(x => x.DisplayOrder).Select(x => new ListEntry()
            {
                Text = x.Abbreviation,
                Value = x.Id.ToString(),
                Attributes = new { data_increment = x.DefaultIncrement }
            }).OrderBy(x => x.Text).ToList();

        }
    }
}
