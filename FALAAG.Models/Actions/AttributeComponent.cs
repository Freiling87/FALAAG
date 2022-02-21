namespace FALAAG.Models
{
	public class AttributeComponent
	{
		public AttributeKey AttributeKey { get; set; }
		public float Percent { get; set; }

		public AttributeComponent(AttributeKey attributeType, float percent)
		{
			AttributeKey = attributeType;
			Percent = percent;
		}
	}
}