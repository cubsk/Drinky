package net.codegoggles.drinky.recipelist;

import java.util.ArrayList;

import java.util.List;

import net.codegoggles.drinky.BaseActivity;
import net.codegoggles.drinky.R;
import net.codegoggles.drinky.R.id;
import net.codegoggles.drinky.R.layout;
import net.codegoggles.drinky.R.menu;
import net.codegoggles.drinky.data.RecipeListItemModel;
import net.codegoggles.drinky.data.RecipeListModel;

import android.content.Intent;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;
import android.widget.Toast;
import android.view.View;
import android.view.ViewGroup;
import com.octo.android.robospice.persistence.DurationInMillis;
import com.octo.android.robospice.persistence.exception.SpiceException;
import com.octo.android.robospice.request.listener.RequestListener;
import android.os.Bundle;
import android.view.Menu;
import android.view.inputmethod.EditorInfo;
import android.widget.ListView;
import android.view.InputEvent;
import net.codegoggles.drinky.recipeview.*;
import net.codegoggles.drinky.*;

public class RecipeListActivity extends BaseActivity {

    
    private RecipeListRequest recipeRequest;
    private ListView listView;
    private TextView filterText;
    
    private RecipeAdapter adapter;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_recipe_list);
		
		recipeRequest = new RecipeListRequest("");
		
		listView = (ListView) findViewById(R.id.recipelist);
		adapter = new RecipeAdapter(this,new ArrayList<RecipeViewModel>());
		listView.setAdapter(adapter);
		
		filterText = (TextView)findViewById(R.id.recipe_filter);
		filterText.setOnEditorActionListener(new OnEditorActionListener() {
		    @Override
		    public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
		        boolean handled = false;
		        if (actionId == EditorInfo.IME_ACTION_SEND) {
		            handled = true;
		            String filterValue = filterText.getText().toString();
		            recipeRequest = new RecipeListRequest(filterValue);
			        getSpiceManager().execute( recipeRequest, "json", DurationInMillis.ONE_SECOND, new RecipeListRequestListener() );
			        Toast.makeText( RecipeListActivity.this, "filter applied: " + filterValue, Toast.LENGTH_SHORT ).show();
		        }
		        return handled;
		    }
		});
		
		listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
			  @Override
			  public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			  		RecipeViewModel item = adapter.getItem(position);
			  		String recipeId = item.getId();
			  		viewRecipe(recipeId);
			  }
		}); 
	}

	private void viewRecipe(String recipeId) {
		
		  Intent intent = new Intent(this, RecipeViewActivity.class);
		  startActivity(intent);
	}
	
	@Override
    protected void onStart() {
        super.onStart();

        setProgressBarIndeterminate( false );
        setProgressBarVisibility( true );

        getSpiceManager().execute( recipeRequest, "json", DurationInMillis.ONE_MINUTE, new RecipeListRequestListener() );
	}
	  
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.recipe_list, menu);
		return true;
	}

	
	 public final class RecipeListRequestListener implements RequestListener<RecipeListModel> {

	        @Override
	        public void onRequestFailure( SpiceException spiceException ) {
	            Toast.makeText( RecipeListActivity.this, "failure", Toast.LENGTH_SHORT ).show();
	        }

	        @Override
	        public void onRequestSuccess( final RecipeListModel result ) {
	            

    			ArrayList<RecipeViewModel> recipeModels = new ArrayList<RecipeViewModel>();
    			
    			for(int i = 0; i < result.size(); i++) {
    				RecipeListItemModel item = result.get(i);
    				
    				recipeModels.add(new RecipeViewModel(item.getId(), item.getName(), item.isIsFavorite()));
    			}
    			int size = recipeModels.size();
    			
	    		adapter.clear();
	    		adapter.addAll(recipeModels);
	    		adapter.notifyDataSetChanged();
	    		Toast.makeText( RecipeListActivity.this, "updated recipe list: " + size, Toast.LENGTH_SHORT ).show();
	        }
	 }
	
}
