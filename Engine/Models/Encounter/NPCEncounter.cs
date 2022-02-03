namespace Engine.Models
{
    public class NPCEncounter
    {
        public string NpcID { get; }
        public int ChanceOfEncountering { get; set; }

        public NPCEncounter(string npcID, int chanceOfEncountering)
        {
            NpcID = npcID;
            ChanceOfEncountering = chanceOfEncountering;
        }
    }
}