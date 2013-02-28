
$(document).on("change", "#addIngredientToRecipe input[type='radio'][name='UnitOfMeasureId']", function (event, ui) {
    var increment = $(this).attr("data-increment");
    var quantity = $("#addIngredientToRecipe [name=Quantity]");
    quantity.attr("data-increment", increment);
    quantity.val("0");
});

$(document).on("click", "#addIngredientToRecipe .quantity-minus" ,function () {
    var quantityField = $("#addIngredientToRecipe [name=Quantity]");
    var increment = parseFloat(quantityField.attr("data-increment"));
    var quantity = parseFloat(quantityField.val());
    var newValue = quantity - increment;
    if (newValue < 0) newValue = 0;
    quantityField.val(newValue);

});



$(document).on("click", "#addIngredientToRecipe .quantity-plus", function () {
    var quantityField = $("#addIngredientToRecipe [name=Quantity]");
    var increment = parseFloat(quantityField.attr("data-increment"));
    var quantity = parseFloat(quantityField.val());
    var newValue = quantity + increment;
    quantityField.val(newValue);
});

$(document).on("change", "#addIngredientToRecipe [name='IngredientTypeId']", function (event, ui) {
    $.mobile.showPageLoadingMsg();
    var ingredientTypeId = $(this).val();
    if (ingredientTypeId == "none") {
        $("#addIngredientToRecipe").find("[name='IngredientId']").selectmenu("disable");
        return;
    }
    $.get('/Recipes/IngredientsByType/?IngredientTypeId=' + ingredientTypeId, function (data) {
        $("#addIngredientToRecipe").find("[name='IngredientId']").html($(data).find("option"));
        $("#addIngredientToRecipe").find("[name='IngredientId']").selectmenu("refresh");
        $("#addIngredientToRecipe").find("[name='IngredientId']").selectmenu("enable");
        $.mobile.hidePageLoadingMsg();

    });


});

$(document).on("click", "#addIngredientToRecipe [action=delete]", function () {
    var componentId = $("#addIngredientToRecipe").find("input[name$=ComponentId]").val();
    var componentList = $("ul#ingredients_listing");
    var existingComponent = componentList.find("li[data-component-id='" + componentId + "']");
    existingComponent.remove();
    componentList.listview('refresh');

});

$(document).on("click", "#addIngredientToRecipe [action=save]", function () {
    var form = $("#addIngredientToRecipe form");

    $.ajax({
        type: "POST",
        url: form.attr("action"),
        data: form.serialize(),
        dataType: "html",
        cache: false,
        success: function (data, textStatus, jqXHR) {
            // detect if our returned result is a list item or the edit page
            var newListItem = $(data);

            if (newListItem.is(".recipe-component-list-item")) {
                var componentList = $("ul#ingredients_listing");
                var componentId = newListItem.attr("data-component-id");
                var existingComponent = componentList.find("li[data-component-id='" + componentId + "']");
                if(existingComponent.length > 0) {
                    existingComponent.replaceWith(newListItem);
                } else {
                    componentList.append(newListItem);

                }
                $("#addIngredientToRecipe").dialog("close");
                componentList.listview('refresh');
            } else {
                var updatedContent = $(data).find("div[data-role=content]").children();
                var content = $("#addIngredientToRecipe div[data-role=content]");
                content.empty();
                updatedContent.appendTo(content).trigger("create");
            }

        },
        error: function (xhr, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

    return false;

});

$(document).on("click", ".recipe-favorite-action a", function () {


    var parentMenu = $(this).parents("div.recipe-menu");
    var parentItem = $(this).parents("li.recipe-favorite-action");

    var recipeId = parentMenu.attr("id");

    var action = "add";
    if (parentItem.hasClass("remove-favorite")) {
        action = "remove";
    }

    $.mobile.loading('show');

    switch (action) {
        case "add":
            $(".recipe-list li[recipe-id='" + recipeId + "'] img.recipe-favorite-status").attr("src", "/Content/Images/star-full.png");
            parentMenu.removeClass("is-not-favorite").addClass("is-favorite");
            $.ajax({
                type: "POST",
                url: "/Recipes/MakeFavorite/",
                data: { RecipeId: recipeId },
                dataType: "json",
                cache: false,
                success: function (data, textStatus, jqXHR) {
                    $.mobile.loading('hide');
                    parentMenu.popup("close");
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
            break;
        case "remove":
            $(".recipe-list li[recipe-id='" + recipeId + "'] img.recipe-favorite-status").attr("src", "/Content/Images/star-empty.png");
            parentMenu.removeClass("is-favorite").addClass("is-not-favorite");
            $.ajax({
                type: "POST",
                url: "/Recipes/RemoveFromFavorites/",
                data: { RecipeId: recipeId },
                dataType: "json",
                cache: false,
                success: function (data, textStatus, jqXHR) {
                    $.mobile.loading('hide');
                    parentMenu.popup("close");
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
            break;
    }
    return false;

});


