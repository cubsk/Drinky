using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrinkyMobile.Models.Ingredients;
using Drinky.DAC;
using NHibernate.Linq;
using Drinky.MVC;


namespace DrinkyMobile.Controllers
{
    public class IngredientsController : Controller
    {

        public ActionResult List()
        {
            IngredientListModel model = new IngredientListModel();

            var session = SessionManager.GetSession();


            var ingredients = session.Query<Ingredient>().OrderBy(x => x.Name).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                IngredientTypeName = x.Type.Name,
                IngredientTypeId = x.Type.Id
            }).ToList();

            foreach (var ingredientType in ingredients
                .GroupBy(x=> new { Id = x.IngredientTypeId, Name = x.IngredientTypeName})
                .Where(x=> x.Count() > 0)
                .OrderBy(y => y.Key.Name))
            {
                IngredientListItemTypeGroupModel typeGroup = new IngredientListItemTypeGroupModel();
                typeGroup.TypeId = ingredientType.Key.Id;
                typeGroup.TypeName = ingredientType.Key.Name;

                foreach (var ingredient in ingredientType.OrderBy(x=> x.Name))
                {
                    typeGroup.Ingredients.Add(new IngredientListItemModel()
                    {
                        Id = ingredient.Id,
                        Name = ingredient.Name
                    });
                }
                model.IngredientGroups.Add(typeGroup);
            }


            ViewData.Model = model;
            return View();
        }

        public ActionResult View(Guid id)
        {
            var session = SessionManager.GetSession();
            var ingredient = session.Load<Ingredient>(id);


            IngredientViewModel model = new IngredientViewModel();
            model.Id = id;
            model.TypeName = ingredient.Type.Name;
            model.Name = ingredient.Name;
            model.Description = ingredient.Description;

            ViewData.Model = model;
            return View();
        }
   
        public ActionResult Add()
        {
            using(NHTransaction transaction =  SessionManager.BeginTransaction())
            {
                IngredientModel model = new IngredientModel();
                model.IngredientTypes = GetIngredientType();
                ViewData.Model = model;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Add(IngredientModel ingredientModel)
        {
            using (var transaction = SessionManager.BeginTransaction())
            {
                ViewData.Model = ingredientModel;
                ingredientModel.IngredientTypes = GetIngredientType();
                ViewData.Model = ingredientModel;

                if (ValidateAdd(ingredientModel))
                {
                    Ingredient newIngredient = new Ingredient() {
                      Name = ingredientModel.Name,
                      Type = new IngredientType() { Id = new Guid(ingredientModel.IngredientType)}
                    };
                    transaction.Session.Save(newIngredient);
                    transaction.Commit();
                    return Redirect("/Ingredients/List/");
                }
                else
                    return View();
            }
        }

        public ActionResult Edit(Guid id)
        {
            using (NHTransaction transaction = SessionManager.BeginTransaction())
            {
                var ingredient = transaction.Session.Load<Ingredient>(id);
                IngredientModel model = new IngredientModel();
                model.IngredientTypes = GetIngredientType();
                model.IngredientType = ingredient.Type.Id.ToString();
                model.Description = ingredient.Description;
                model.Name = ingredient.Name;
                ViewData.Model = model;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(IngredientModel ingredientModel)
        {
            using (var transaction = SessionManager.BeginTransaction())
            {

                ViewData.Model = ingredientModel;
                ingredientModel.IngredientTypes = GetIngredientType();
                ViewData.Model = ingredientModel;

                if (ValidateEdit(ingredientModel))
                {
                    var ingredient = transaction.Session.Load<Ingredient>(ingredientModel.Id);
                    ingredient.Name = ingredientModel.Name;
                    ingredient.Type = new IngredientType() { Id = new Guid(ingredientModel.IngredientType) };
                    ingredient.Description = ingredientModel.Description;
                    transaction.Session.SaveOrUpdate(ingredient);
                    transaction.Commit();
                    return Redirect("/Ingredients/List/");
                }
                else
                    return View();
            }
        }

        public ActionResult ConfirmDelete(Guid Id)
        {
            IngredientModel model = new IngredientModel();
            model.Id = Id;
            ViewData.Model = model;
            return View();
        }

        public ActionResult Delete(Guid Id)
        {
            using (NHTransaction transaction = SessionManager.BeginTransaction())
            {
                var ingredient = transaction.Session.Get<Ingredient>(Id);
                transaction.Session.Delete(ingredient);
                transaction.Commit();
                return View();
            }
        }



        protected bool ValidateAdd(IngredientModel model)
        {
            var validator = new Validator<IngredientModel>(model, ViewData.ModelState);
            validator.RequireValue(x => x.Name, "Ingredient name is required");
            validator.RequireValue(x => x.IngredientType, "Please select an ingredient type");

            if (SessionManager.GetSession().Query<Ingredient>().Any(x => x.Name == model.Name))
                validator.AddError(x => x.Name, "An ingredient with this name already exists");

            return ViewData.ModelState.IsValid;
        }

        protected bool ValidateEdit(IngredientModel model)
        {
            var validator = new Validator<IngredientModel>(model, ViewData.ModelState);
            validator.RequireValue(x => x.Name, "Ingredient name is required");
            validator.RequireValue(x => x.IngredientType, "Please select an ingredient type");

            if (SessionManager.GetSession().Query<Ingredient>().Any(x => x.Name == model.Name && x.Id != model.Id))
                validator.AddError(x => x.Name, "An ingredient with this name already exists");

            return ViewData.ModelState.IsValid;
        }

        protected List<ListEntry> GetIngredientType()
        {
            return SessionManager.GetSession().Query<IngredientType>().Select(x => new ListEntry()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text).WithNoneSelected().ToList();

        }

        protected List<ListEntry> GetMeasureType()
        {
            return SessionManager.GetSession().Query<UnitOfMeasure>().OrderBy(x => x.DisplayOrder).Select(x => new ListEntry()
            {
                Text = x.Name ,
                Value = x.Id.ToString()
            }).OrderBy(x => x.Text).ToList().WithNoneSelected();

        }

    }
}
