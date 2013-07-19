package net.codegoggles.drinky.recipelist;

import java.util.ArrayList;

import net.codegoggles.drinky.R;
import net.codegoggles.drinky.R.drawable;
import net.codegoggles.drinky.R.id;
import net.codegoggles.drinky.R.layout;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class RecipeAdapter extends ArrayAdapter<RecipeListItemViewModel>{

	private final Context context;
	private final ArrayList<RecipeListItemViewModel> recipes;
	  
	public RecipeAdapter(Context context, ArrayList<RecipeListItemViewModel> recipes) {
		super(context, R.layout.activity_recipe_row, recipes);
		this.context = context;
		this.recipes = recipes;
		
	}
	
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        if (convertView == null) {
            convertView = inflater.inflate(R.layout.activity_recipe_row, parent, false);
        } 

	    TextView textView = (TextView) convertView.findViewById(R.id.label);
	    ImageView imageView = (ImageView) convertView.findViewById(R.id.icon);
	
	    RecipeListItemViewModel r = recipes.get(position);
	    textView.setText(r.getName());
	    
	    if(r.getIsFavorite()) {
	    	imageView.setImageResource(R.drawable.star_full);
	    } else {
	    	imageView.setImageResource(R.drawable.star_empty);
	    }
	    
	    return convertView;
	}
	
}
