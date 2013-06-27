package net.codegoggles.drinky;

import java.util.ArrayList;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class RecipeAdapter extends ArrayAdapter<RecipeModel>{

	private final Context context;
	private final ArrayList<RecipeModel> recipes;
	  
	public RecipeAdapter(Context context, ArrayList<RecipeModel> recipes) {
		super(context, R.layout.activity_recipe_row, recipes);
		this.context = context;
		this.recipes = recipes;
		
	}
	
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		View rowView = inflater.inflate(R.layout.activity_recipe_row, parent, false);
		
	    TextView textView = (TextView) rowView.findViewById(R.id.label);
	    ImageView imageView = (ImageView) rowView.findViewById(R.id.icon);
	
    	RecipeModel r = recipes.get(position);
	    textView.setText(r.getName());
	    
	    if(r.getIsFavorite()) {
	    	imageView.setImageResource(R.drawable.star_full);
	    } else {
	    	imageView.setImageResource(R.drawable.star_empty);
	    }
	    
	    return rowView;
	}
	
}
