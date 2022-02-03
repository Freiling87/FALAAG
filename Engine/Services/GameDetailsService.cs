using System.IO;
using Engine.Models;
using Engine.Shared;
using Newtonsoft.Json.Linq;

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
                        DisplayName = token.StringValueOf("DisplayName")
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
            }

            return gameDetails;
        }
    }
}