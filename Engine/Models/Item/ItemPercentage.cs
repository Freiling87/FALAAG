namespace Engine.Models
{
    public class ItemPercentage
    {
        public string ID { get; }
        public int Percentage { get; }

        public ItemPercentage(string id, int percentage)
        {
            ID = id;
            Percentage = percentage;
        }
    }
}