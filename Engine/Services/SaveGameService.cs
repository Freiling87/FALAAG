using System;
using System.IO;
using Engine.Factories;
using Engine.Models;
using Engine.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Engine.Services
{
    public static class SaveGameService
    {
        public static void Save(GameSession gameSession, string filePath) =>
            File.WriteAllText(filePath, 
                JsonConvert.SerializeObject(gameSession, Formatting.Indented));

        public static GameSession LoadLastSaveOrCreateNew(string filePath)
        {
            if (!File.Exists(filePath)) 
                throw new FileNotFoundException($"Filename: {filePath}");

			try
            {
                JObject data = JObject.Parse(File.ReadAllText(filePath));
                Player player = CreatePlayer(data);
                int x = (int)data[nameof(GameSession.CurrentCell)][nameof(Cell.X)];
                int y = (int)data[nameof(GameSession.CurrentCell)][nameof(Cell.Y)];
                int z = (int)data[nameof(GameSession.CurrentCell)][nameof(Cell.Z)];
                return new GameSession(player, x, y, z);
            }
            catch
			{
                throw new FormatException($"Error reading: {filePath}");
            }
        }

        private static Player CreatePlayer(JObject data)
        {
            Player player =
                new Player((string)data[nameof(GameSession.Player)][nameof(Player.Name)],
                            "Player",
                           (int)data[nameof(GameSession.Player)][nameof(Player.Xp)],
                           (int)data[nameof(GameSession.Player)][nameof(Player.HpMax)],
                           (int)data[nameof(GameSession.Player)][nameof(Player.HpCur)],
                           GetPlayerAttributes(data),
                           (int)data[nameof(GameSession.Player)][nameof(Player.Cash)]);

            PopulatePlayerInventory(data, player);
            PopulatePlayerJobs(data, player);
            PopulatePlayerRecipes(data, player);
            return player;
        }

        private static IEnumerable<EntityAttribute> GetPlayerAttributes(JObject data)
        {
            List<EntityAttribute> attributes =
                new List<EntityAttribute>();

            foreach (JToken itemToken in (JArray)data[nameof(GameSession.Player)]
                [nameof(Player.Attributes)])
            {
                attributes.Add(new EntityAttribute(
                                   (string)itemToken[nameof(EntityAttribute.Key)],
                                   (string)itemToken[nameof(EntityAttribute.DisplayName)],
                                   (string)itemToken[nameof(EntityAttribute.DiceNotation)],
                                   (int)itemToken[nameof(EntityAttribute.BaseValue)],
                                   (int)itemToken[nameof(EntityAttribute.ModifiedValue)]));
            }

            return attributes;
        }
        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            foreach (JToken itemToken in (JArray)data
                [nameof(GameSession.Player)]
                            [nameof(Player.Inventory)]
                                   [nameof(Inventory.Items)])
            {
                string itemId = (string)itemToken[nameof(Item.ID)];
                player.InventoryAddItem(ItemFactory.CreateItem(itemId));
            }
        }
        private static void PopulatePlayerJobs(JObject data, Player player)
        {
            foreach (JToken JobToken in (JArray)data
                [nameof(GameSession.Player)]
                            [nameof(Player.JobsActive)])
            {
                string ID = (string)JobToken[nameof(JobStatus.Job)][nameof(Job.ID)];
                Job job = JobFactory.GetJobByID(ID);
                JobStatus jobStatus = new JobStatus(job);
                jobStatus.IsCompleted = (bool)JobToken[nameof(jobStatus.IsCompleted)];
                player.JobsActive.Add(jobStatus);
            }
        }
        private static void PopulatePlayerRecipes(JObject data, Player player)
        {
            foreach (JToken recipeToken in (JArray)data
                [nameof(GameSession.Player)]
                            [nameof(Player.RecipesKnown)])
            {
                string recipeId = (string)recipeToken[nameof(Recipe.ID)];
                Recipe recipe = RecipeFactory.RecipeByID(recipeId);
                player.RecipesKnown.Add(recipe);
            }
        }
    }
}