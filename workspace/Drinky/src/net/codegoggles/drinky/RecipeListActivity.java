package net.codegoggles.drinky;

import java.util.ArrayList;
import java.util.List;

import android.os.Bundle;
import android.view.Window;
import android.widget.TextView;
import android.widget.Toast;

import com.octo.android.robospice.persistence.DurationInMillis;
import com.octo.android.robospice.persistence.exception.SpiceException;
import com.octo.android.robospice.request.listener.RequestListener;
import android.os.Bundle;
import android.view.Menu;
import android.widget.ListView;

public class RecipeListActivity extends BaseActivity {

    
    private RecipeListRequest recipeRequest;
    private ListView listView;
    private RecipeAdapter adapter;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_recipe_list);
		
		recipeRequest = new RecipeListRequest("");
		
		listView = (ListView) findViewById(R.id.recipelist);
		adapter = new RecipeAdapter(this,new ArrayList<RecipeModel>());
		listView.setAdapter(adapter);
		
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
	            Toast.makeText( RecipeListActivity.this, "success", Toast.LENGTH_SHORT ).show();

	    		final ListView listview = (ListView) findViewById(R.id.recipelist);

    			ArrayList<RecipeModel> recipeModels = new ArrayList<RecipeModel>();
    			
    			for(int i = 0; i < result.size(); i++) {
    				RecipeListItemModel item = result.get(i);
    				
    				recipeModels.add(new RecipeModel(item.getId(), item.getName(), item.isIsFavorite()));
    			}
    			
	    		adapter.clear();
	    		adapter.addAll(recipeModels);
	    		adapter.notifyDataSetChanged();
	    		
	        }
	 }
	
}
