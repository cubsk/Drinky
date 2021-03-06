package net.codegoggles.drinky.recipeview;

import net.codegoggles.drinky.data.*;
import net.codegoggles.drinky.BaseActivity;
import net.codegoggles.drinky.R;
import net.codegoggles.drinky.R.layout;
import net.codegoggles.drinky.R.menu;
import android.os.Bundle;
import android.app.Activity;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.TextView;
import android.support.v4.app.NavUtils;


public class RecipeViewActivity extends BaseActivity {

	private TextView recipeName;
	private TextView recipeGlassware;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_recipe_view);
		setupActionBar();
		
		RecipeModel recipe = new RecipeModel();
		recipe.Name = "View Mot";
		recipe.Preperation = "Dry shake ingredients to combine, then shake well with cracked ice. Strain over fresh ice in an old fashioned glass. Garnish with lemon peel and brandied cherries, if desired. Serve and grin like an idiot as your friends freak out.";
		recipe.GlasswareType = "Coupe Glass";
		
		RecipeComponentModel ingredientOne = new RecipeComponentModel();
		ingredientOne.IngredientName = "DiSarrono Amaretto";
		
		
		recipe.Ingredients.add(new RecipeComponentModel());
		
		recipeName = (TextView)findViewById(R.id.nameValue);
		recipeName.setText("View Mot");
		
		recipeGlassware = (TextView)findViewById(R.id.glasswareValue);
		recipeGlassware.setText("Coupe Glass");
	}


	private void setupActionBar() {
		getActionBar().setDisplayHomeAsUpEnabled(true);

	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.recipe_view, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		switch (item.getItemId()) {
		case android.R.id.home:

			NavUtils.navigateUpFromSameTask(this);
			return true;
		}
		return super.onOptionsItemSelected(item);
	}

}
