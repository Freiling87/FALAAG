using System.IO;
using Engine.Models;
using Engine.Shared;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Engine.Services
{
    public static class GameDetailsService
    {
        public static GameDetails ReadGameDetails()
        {
            JObject gameDetailsJson =
                JObject.Parse(File.ReadAllText(".\\GameData\\GameDetails.json"));

            GameDetails gameDetails =
                new GameDetails(gameDetailsJson.StringValueOf("Title"),
                                gameDetailsJson.StringValueOf("Subtitle"),
                                gameDetailsJson.StringValueOf("Version"));

            foreach (JToken token in gameDetailsJson["Attributes"])
            {
                gameDetails.Attributes.Add(new EntityAttribute(token.StringValueOf("Key"),
                                                                     token.StringValueOf("DisplayName"),
                                                                     token.StringValueOf("DiceNotation")));
            }

            if (gameDetailsJson["Archetypes"] != null)
            {
                foreach (JToken token in gameDetailsJson["Archetypes"])
                {
                    Archetype archetype = new Archetype
                    {
                        Key = token.StringValueOf("Key"),
                        Description = token.StringValueOf("Description"),
                        DisplayName = token.StringValueOf("DisplayName"),
                        Group = token.StringValueOf("Group"),
                    };

                    if (token["AttributeModifiers"] != null)
                    {
                        foreach (JToken childToken in token["AttributeModifiers"])
                        {
                            archetype.AttributeModifiers.Add(new AttributeModifier
                            {
                                Key = childToken.StringValueOf("Key"),
                                Modifier = childToken.IntValueOf("Modifier")
                            });
                        }
                    }

                    gameDetails.Archetypes.Add(archetype);
                }

                gameDetails.BodyTypes = gameDetails.Archetypes.Where(a => a.Group == "Body").ToList();
                gameDetails.MindTypes = gameDetails.Archetypes.Where(a => a.Group == "Mind").ToList();
                gameDetails.PersonaTypes = gameDetails.Archetypes.Where(a => a.Group == "Persona").ToList();
                gameDetails.Races = gameDetails.Archetypes.Where(a => a.Group == "Race").ToList();
                gameDetails.Sexes = gameDetails.Archetypes.Where(a => a.Group == "Sex").ToList();
                gameDetails.SpiritTypes = gameDetails.Archetypes.Where(a => a.Group == "Spirit").ToList();

            }

            return gameDetails;
        }
    }
}