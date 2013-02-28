using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drinky.MVC;
using System.Web.Mvc;
using NHibernate.Linq;
using Drinky.DAC;
namespace DrinkyMobile.Models.IngredientTypes
{
    public class IngredientTypeValidator : Validator<IngredientTypeEditModel>
    {

        public IngredientTypeValidator(IngredientTypeEditModel model, ModelStateDictionary modelState, string rootPath)
            : base(model, modelState, rootPath) { }

        public IngredientTypeValidator(IngredientTypeEditModel model, ModelStateDictionary modelState)
            : base(model, modelState) { }


        public bool ValidateAdd()
        {
            var session = SessionManager.GetSession();


            RequireValue(x => x.Name, "Please provide a name");

            if (session.Query<IngredientType>().Any(x => x.Name == Model.Name ))
                AddError(x => x.Name, "Another ingredient type with this name already exists");

            return ModelState.IsValid;

        }

        public bool ValidateEdit()
        {
            var session = SessionManager.GetSession();

            RequireValue(x => x.Name, "Please provide a name");

            if (session.Query<IngredientType>().Any(x => x.Name == Model.Name && x.Id != Model.Id.Value))
                AddError(x => x.Name, "Another ingredient type with this name already exists");

            return ModelState.IsValid;


        }
    }
}