namespace FALAAG.Models
{
	public class AttributeComponent
	{
		public string Key { get; set; }
		public float Percent { get; set; }

		public AttributeComponent(string key, float percent)
		{
			Key = key;
			Percent = percent;
		}
	}
}