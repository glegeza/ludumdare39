namespace DLS.LD39.Equipment
{
    public abstract class Loot
    {
        protected Loot(string name, string desc, LootType type)
        {
            Name = name;
            Description = desc;
        }

        public string Name
        {
            get; private set;
        }

        public string Description
        {
            get; private set;
        }

        public LootType Type
        {
            get; private set;
        }
    }
}