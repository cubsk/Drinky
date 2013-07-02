package net.codegoggles.drinky;


import com.google.api.client.util.Key;

public class RecipeListItemModel {

	@Key
	private String Id;
	@Key
	private String Name;
	@Key
	private boolean IsFavorite;
	
	
	public String getId() {
		return Id;
	}
	public void setId(String id) {
		Id = id;
	}
	public String getName() {
		return Name;
	}
	public void setName(String name) {
		Name = name;
	}
	public boolean isIsFavorite() {
		return IsFavorite;
	}
	public void setIsFavorite(boolean isFavorite) {
		IsFavorite = isFavorite;
	}
	
	 @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + ( Id == null ? 0 : Id.hashCode() );
        result = prime * result + ( Name == null ? 0 : Name.hashCode() );
        result = prime * result + ( IsFavorite == false ? 0 : 1);
        return result;
    }
	
	 @Override
	    public boolean equals( Object obj ) {
	        if ( this == obj ) {
	            return true;
	        }
	        if ( obj == null ) {
	            return false;
	        }
	        if ( getClass() != obj.getClass() ) {
	            return false;
	        }
	        RecipeListItemModel other = (RecipeListItemModel) obj;
	        if ( Id == null ) {
	            if ( other.Id != null ) {
	                return false;
	            }
	        } else if ( !Id.equals( other.Id ) ) {
	            return false;
	        }
	        if ( Name == null ) {
	            if ( other.Name != null ) {
	                return false;
	            }
	        } else if ( !Name.equals( other.Name ) ) {
	            return false;
	        }
	        if ( !Name.equals( other.Name ) ) {
	            return false;
	        }
	        return true;
	    }
}
