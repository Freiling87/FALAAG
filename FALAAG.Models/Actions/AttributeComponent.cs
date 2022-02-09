namespace FALAAG.Models
{
	public class AttributeComponent
	{
		public string Key { get; set; }
		public int Percent { get; set; }

		public AttributeComponent(string key, int percent)
		{
			Key = key;
			Percent = percent;
		}
	}
}