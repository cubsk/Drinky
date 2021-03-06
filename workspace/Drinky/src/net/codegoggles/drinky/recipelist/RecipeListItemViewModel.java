package net.codegoggles.drinky.recipelist;


public class RecipeListItemViewModel {

	private String Id;
	private String Name;
	private Boolean IsFavorite;
	
	public String getName() {
		return Name;
	}

	public boolean getIsFavorite() {
		return IsFavorite;
	}

	public void setIsFavorite(Boolean isFavorite) {
		IsFavorite = isFavorite;
	}

	public void setName(String name) {
		Name = name;
	}
	
	public RecipeListItemViewModel(String Id, String name, boolean isFavorite)
	{
		this.Id = Id;
		this.Name = name;
		this.IsFavorite = isFavorite;
		
	}

	public String getId() {
		return Id;
	}

	public void setId(String id) {
		Id = id;
	}
}
