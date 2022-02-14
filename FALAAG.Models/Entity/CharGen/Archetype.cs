using System.Collections.Generic;

namespace FALAAG.Models
{
    public enum ArchetypeGroup
	{
        Body,
        Mind,
        Persona,
        Race,
        Sex,
        Spirit
	}

    public class Archetype
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ArchetypeGroup ArchetypeGroup { get; set; }
        public List<AttributeModifier> AttributeModifiers { get; } = new List<AttributeModifier>();
		public List<SkillModifier> SkillModifiers { get; set; }

        public Archetype(string id, string displayName, string description, ArchetypeGroup archetypeGroup)
		{
            ID = id;
            DisplayName = displayName;
            Description = description;
            ArchetypeGroup = archetypeGroup;
		}
	}
}