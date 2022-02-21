using System.IO;
using FALAAG.Models;
using FALAAG.Models.Shared;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace FALAAG.Services
{
    public static class GameDetailsService
    {
        public static GameDetails ReadGameDetails()
        {
            JObject gameDetailsJson =
                JObject.Parse(File.ReadAllText(".\\GameData\\GameDetails.json"));

            GameDetails gameDetails = new (
                gameDetailsJson.StringValueOf("Title"),
                gameDetailsJson.StringValueOf("Subtitle"),
                gameDetailsJson.StringValueOf("Version"));

            foreach (JToken token in gameDetailsJson["Attributes"])
            {
                gameDetails.Attributes.Add(new EntityAttribute(
                    Enum.Parse<AttributeKey>(token.StringValueOf("Key")),
                    token.StringValueOf("DisplayName"),
                    token.StringValueOf("Description"),
                    token.StringValueOf("DiceNotation")));
            }

            foreach (JToken token in gameDetailsJson["Skills"])
            {
                Skill Skill = new (
                    token.StringValueOf("ID"),
                    token.StringValueOf("Name"),
                    token.StringValueOf("Description"));

                if (token["AttributeComponents"] != null)
                    foreach (JToken childToken in token["AttributeComponents"])
                        Skill.AttributeComponents.Add(new AttributeComponent(
                            Enum.Parse<AttributeKey>(childToken.StringValueOf("Key")),
                            childToken.IntValueOf("Percent")));

                gameDetails.Skills.Add(Skill);
            }

            foreach (JToken token in gameDetailsJson["Archetypes"])
            {
                Archetype archetype = new Archetype(
                    token.StringValueOf("Key"),
                    token.StringValueOf("DisplayName"),
                    token.StringValueOf("Description"),
                    Enum.Parse<ArchetypeGroup>(token.StringValueOf("Group")));

                if (token["AttributeModifiers"] != null)
                    foreach (JToken childToken in token["AttributeModifiers"])
                        archetype.AttributeModifiers.Add(new AttributeModifier(
                            childToken.StringValueOf("Key"),
                            childToken.IntValueOf("Modifier")));

                gameDetails.Archetypes.Add(archetype);
            }

            gameDetails.BodyTypes = gameDetails.Archetypes.Where(a => a.ArchetypeGroup == ArchetypeGroup.Body).ToList();
            gameDetails.MindTypes = gameDetails.Archetypes.Where(a => a.ArchetypeGroup == ArchetypeGroup.Mind).ToList();
            gameDetails.PersonaTypes = gameDetails.Archetypes.Where(a => a.ArchetypeGroup == ArchetypeGroup.Persona).ToList();
            gameDetails.Races = gameDetails.Archetypes.Where(a => a.ArchetypeGroup == ArchetypeGroup.Race).ToList();
            gameDetails.Sexes = gameDetails.Archetypes.Where(a => a.ArchetypeGroup == ArchetypeGroup.Sex).ToList();
            gameDetails.SpiritTypes = gameDetails.Archetypes.Where(a => a.ArchetypeGroup == ArchetypeGroup.Spirit).ToList();

            return gameDetails;
        }
    }
}