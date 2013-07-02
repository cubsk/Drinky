package net.codegoggles.drinky.recipelist;

import java.io.IOException;
import java.util.List;

import net.codegoggles.drinky.data.RecipeListModel;



import roboguice.util.temp.Ln;

import com.google.api.client.http.GenericUrl;
import com.google.api.client.http.HttpRequest;
import com.google.api.client.json.jackson.JacksonFactory;
import com.octo.android.robospice.request.googlehttpclient.GoogleHttpClientSpiceRequest;


public class RecipeListRequest extends GoogleHttpClientSpiceRequest<RecipeListModel>{

	 private String baseUrl;

	    public RecipeListRequest( String filter ) {
	        super( RecipeListModel.class );
	        this.baseUrl = "http://drinkyapi.azurewebsites.net/api/recipe?filter=" + filter;
	    }

	    @Override
	    public RecipeListModel loadDataFromNetwork() throws IOException {
	        Ln.d( "Call web service " + baseUrl );
	        HttpRequest request = getHttpRequestFactory().buildGetRequest( new GenericUrl( baseUrl ) );
	        request.getHeaders().setBasicAuthentication("brian", "Simple01");

            
	        request.setParser( new JacksonFactory().createJsonObjectParser() );
	        return request.execute().parseAs( getResultType() );
	    }
}
