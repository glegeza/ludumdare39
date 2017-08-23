﻿namespace DLS.LD39.Equipment
{
    using DLS.LD39.Actions;
    using System.Collections.Generic;

    public abstract class Loot
    {
        protected Loot(string name, string desc, LootType type, IEnumerable<Action> actions)
        {
            Name = name;
            Description = desc;
            Actions = new List<Action>(actions);
        }

        public string Name
        {
            get; private set;
        }

        public string Description
        {
            get; private set;
        }

        public IEnumerable<Action> Actions
        {
            get; private set;
        }

        public LootType Type
        {
            get; private set;
        }
    }
}