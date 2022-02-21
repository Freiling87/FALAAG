using FALAAG.Factories;
using FALAAG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace FALAAG.Services
{
	public static class SaveGameService
    {
        public static void Save(GameState gameState, string filePath) =>
            File.WriteAllText(filePath, JsonConvert.SerializeObject(gameState, Formatting.Indented));

        public static GameState LoadLastSaveOrCreateNew(string filePath)
        {
            if (!File.Exists(filePath)) 
                throw new FileNotFoundException($"Filename: {filePath}");

			try
            {
                JObject data = JObject.Parse(File.ReadAllText(filePath));
                Player player = CreatePlayer(data);
                int x = (int)data[nameof(GameState.X)];
                int y = (int)data[nameof(GameState.Y)];
                int z = (int)data[nameof(GameState.Z)];
                return new GameState(player, x, y, z);
            }
            catch
			{
                throw new FormatException($"Error reading: {filePath}");
            }
        }

        private static Player CreatePlayer(JObject data)
        {
            Player player = new Player((string)data[nameof(GameState.Player)][nameof(Player.Name)],
                "Player",
                GetPlayerAttributes(data),
                GetPlayerSkills(data));

            PopulatePlayerInventory(data, player);
            PopulatePlayerJobs(data, player);
            PopulatePlayerRecipes(data, player);
            return player;
        }

        private static IEnumerable<EntityAttribute> GetPlayerAttributes(JObject data)
        {
            List<EntityAttribute> attributes = new List<EntityAttribute>();

            foreach (JToken itemToken in (JArray)data[nameof(GameState.Player)][nameof(Player.Attributes)])
            {
                attributes.Add(new EntityAttribute(
                                   Enum.Parse<AttributeKey>((string)itemToken[nameof(EntityAttribute.AttributeKey)]),
                                   (string)itemToken[nameof(EntityAttribute.DisplayName)],
								   (string)itemToken[nameof(EntityAttribute.Description)],
                                   (string)itemToken[nameof(EntityAttribute.DiceNotation)],
                                   (int)itemToken[nameof(EntityAttribute.BaseValue)],
                                   (int)itemToken[nameof(EntityAttribute.ModifiedValue)]
                                   ));
            }

            return attributes;
        }
        private static IEnumerable<Skill> GetPlayerSkills (JObject data)
        {
            List<Skill> skills = new List<Skill>();

            foreach (JToken itemToken in (JArray)data
                [nameof(GameState.Player)][nameof(Player.Skills)])
            {
                skills.Add(new Skill(
                                   (string)itemToken[nameof(Skill.ID)],
                                   (string)itemToken[nameof(Skill.Name)],
                                   (string)itemToken[nameof(Skill.Description)]
                                   ));
            }

            return skills;
        }
        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            foreach (JToken itemToken in (JArray)data
                [nameof(GameState.Player)][nameof(Player.Inventory)][nameof(Inventory.Items)])
            {
                string itemId = (string)itemToken[nameof(Item.ID)];
                player.InventoryAddItem(ItemFactory.CreateItem(itemId));
            }
        }
        private static void PopulatePlayerJobs(JObject data, Player player)
        {
            foreach (JToken JobToken in (JArray)data
                [nameof(GameState.Player)]
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
                [nameof(GameState.Player)]
                            [nameof(Player.RecipesKnown)])
            {
                string recipeId = (string)recipeToken[nameof(Recipe.ID)];
                Recipe recipe = RecipeFactory.RecipeByID(recipeId);
                player.RecipesKnown.Add(recipe);
            }
        }
    }
}