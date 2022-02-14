namespace FALAAG.Models
{
    public class AttributeModifier
    {
        public string ID { get; init; }
        public int Modifier { get; init; }

        public AttributeModifier(string id, int modifier)
		{
            ID = id;
            Modifier = modifier;
		}
    }
}