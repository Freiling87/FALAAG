using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Models;
using Engine.Shared;

namespace Engine.Factories
{
    public static class RecipeFactory
    {
        private const string _contentDataFilepath = ".\\GameData\\Recipes.xml";

        private static readonly List<Recipe> _recipes = new List<Recipe>();

        static RecipeFactory()
        {
            if (File.Exists(_contentDataFilepath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilepath));
                LoadRecipesFromNodes(data.SelectNodes("/Recipes/Recipe"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilepath}");
        }

        private static void LoadRecipesFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Recipe recipe = new Recipe(node.AttributeAsString("ID"), node.AttributeAsString("Name"));

                foreach (XmlNode childNode in node.SelectNodes("./Ingredients/Item"))
                    recipe.AddIngredient(childNode.AttributeAsString("ID"),
                                         childNode.AttributeAsInt("Quantity"));

                foreach (XmlNode childNode in node.SelectNodes("./OutputItems/Item"))
                    recipe.AddOutputItem(childNode.AttributeAsString("ID"),
                                         childNode.AttributeAsInt("Quantity"));

                _recipes.Add(recipe);
            }
        }

        public static Recipe RecipeByID(string id) =>
            _recipes.FirstOrDefault(x => x.ID == id);
    }
}