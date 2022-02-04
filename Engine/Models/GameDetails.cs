using System.Collections.Generic;

namespace Engine.Models
{
    public class GameDetails
    {
        public string Title { get; }
        public string Subtitle { get; }
        public string Version { get; }

        public List<EntityAttribute> Attributes { get; } = new List<EntityAttribute>();
        public List<Archetype> Archetypes { get; }  = new List<Archetype>();

        public GameDetails(string title, string subtitle, string version)
        {
            Title = title;
            Subtitle = subtitle;
            Version = version;
        }
    }
}