using System.Collections.Generic;

namespace Engine.Models
{
    public class Archetype
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Group { get; set; } // Body, Mind, Persona, Spirit
        public List<AttributeModifier> AttributeModifiers { get; } = new List<AttributeModifier>();
    }
}