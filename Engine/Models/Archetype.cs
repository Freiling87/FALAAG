using System.Collections.Generic;

namespace Engine.Models
{
    public class Archetype
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<AttributeModifier> AttributeModifiers { get; } = new List<AttributeModifier>();
    }
}