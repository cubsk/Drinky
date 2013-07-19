package net.codegoggles.drinky.data;

public class RecipeModel {

	public String Id;
	public String Name;
	public String GlasswareType;
	public String GlasswareTypeId;
	public String Preperation;
	
	public RecipeComponentListModel Ingredients;
	
	public RecipeModel()
	{
		this.Ingredients = new RecipeComponentListModel();
	}
}
