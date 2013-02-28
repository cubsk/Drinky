using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;

using System.Web.Mvc;
using Drinky.DAC;
using DrinkyMobile.Models.IngredientTypes;

namespace DrinkyMobile.Controllers
{
    public class IngredientTypesController : Controller
    {

        public ActionResult List()
        {
            var session = SessionManager.GetSession();
            IngredientTypeListModel model = new IngredientTypeListModel();

            model.Items = session.Query<IngredientType>().OrderBy(x => x.Name).Select(x => new IngredientTypeListItemModel()
            {
                 Id = x.Id,
                 Name = x.Name,
                 IngredientCount = x.Ingredients.Count 
            }).ToList();

            ViewData.Model = model;

            return View();
        }

        public ActionResult View(Guid id)
        {
            var session = SessionManager.GetSession();
            IngredientTypeViewModel model = new IngredientTypeViewModel();

            IngredientType ingredientType = session.Get<IngredientType>(id);
            model.Id = ingredientType.Id;
            model.IngredientCount = ingredientType.Ingredients.Count;
            model.Name = ingredientType.Name;


            ViewData.Model = model;
            return View();
        }

        public ActionResult ConfirmDelete(Guid Id)
        {
            var session = SessionManager.GetSession();
            IngredientTypeViewModel model = new IngredientTypeViewModel();
            IngredientType ingredientType = session.Get<IngredientType>(Id);

            model.Id = Id;
            model.IngredientCount = ingredientType.Ingredients.Count;
            model.Name = ingredientType.Name;
            ViewData.Model = model;

            if (ingredientType.Ingredients.Count > 0)
            {
                return View("UnableToDelete");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(Guid Id)
        {
            IngredientTypeViewModel model = new IngredientTypeViewModel();
            ViewData.Model = model;

            using (NHTransaction transaction = SessionManager.BeginTransaction())
            {
                var ingredientType = transaction.Session.Get<IngredientType>(Id);
                model.Id = ingredientType.Id;
                model.Name = ingredientType.Name;

                transaction.Session.Delete(ingredientType);
                transaction.Commit();
                return View();
            }
        }

        public ActionResult Add()
        {
            IngredientTypeEditModel model = new IngredientTypeEditModel();
            ViewData.Model = model;
            return View();

        }

        [HttpPost]
        public ActionResult Add(IngredientTypeEditModel ingredientTypeModel)
        {
            using (var transaction = SessionManager.BeginTransaction())
            {
                var session = SessionManager.GetSession();
                ViewData.Model = ingredientTypeModel;

                IngredientTypeValidator validator = new IngredientTypeValidator(ingredientTypeModel, ViewData.ModelState);
                if (validator.ValidateAdd())
                {
                    IngredientType ingredientType = new IngredientType();
                    ingredientType.Name = ingredientTypeModel.Name;
                    session.SaveOrUpdate(ingredientType);
                    transaction.Commit();
                    return Redirect("/IngredientTypes/List/");

                }

                return View();
            }

        }

        public ActionResult Edit(Guid id)
        {
            var session = SessionManager.GetSession();
            IngredientTypeEditModel model = new IngredientTypeEditModel();

            IngredientType ingredientType = session.Get<IngredientType>(id);
            model.Id = ingredientType.Id;
            model.Name = ingredientType.Name;

            ViewData.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(IngredientTypeEditModel ingredientTypeModel)
        {
            using (var transaction = SessionManager.BeginTransaction())
            {
                var session = SessionManager.GetSession();
                ViewData.Model = ingredientTypeModel;

                IngredientTypeValidator validator = new IngredientTypeValidator(ingredientTypeModel, ViewData.ModelState);
                if (validator.ValidateEdit())
                {
                    IngredientType ingredientType = session.Get<IngredientType>(ingredientTypeModel.Id);
                    ingredientType.Name = ingredientTypeModel.Name;
                    session.SaveOrUpdate(ingredientType);
                    transaction.Commit();
                    return Redirect("/IngredientTypes/List/");
                }

                return View();
            }

        }

    }
}
