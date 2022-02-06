using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class GameDetails
    {
        public string Title { get; }
        public string Subtitle { get; }
        public string Version { get; }

        public List<EntityAttribute> Attributes { get; } = new List<EntityAttribute>();
        public List<Archetype> Archetypes { get; }  = new List<Archetype>();

        public List<Archetype> BodyTypes { get; set; } // Archetypes.Where(a => a.Group == "Body").ToList().
        public List<Archetype> MindTypes { get; set;  }
        public List<Archetype> PersonaTypes { get; set; }
        public List<Archetype> Races { get; set; }
        public List<Archetype> Sexes { get; set; }
        public List<Archetype> SpiritTypes { get; set; }

        public GameDetails(string title, string subtitle, string version)
        {
            Title = title;
            Subtitle = subtitle;
            Version = version;
        }
    }
}