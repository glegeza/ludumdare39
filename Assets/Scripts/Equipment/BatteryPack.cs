﻿namespace DLS.LD39.Equipment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BatteryPack : Loot
    {
        public BatteryPack(string name, string desc) : base(name, desc, LootType.BatteryPack)
        {
        }
    }
}
