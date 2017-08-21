namespace DLS.LD39.Equipment
{
    using DLS.LD39.Combat;
    using DLS.LD39.Map;
    using DLS.LD39.Units;

    public abstract class Weapon : Loot
    {
        protected Weapon(string name, string desc, LootType type, WeaponStats weapon) 
            : base(name, desc, type)
        {
            if (! (type == LootType.PrimaryWeapon || type == LootType.SecondaryWeapon))
            {
                throw new System.Exception("Weapon type must be a primary or secondary weapon");
            }
            if (weapon == null)
            {
                throw new System.ArgumentNullException("weapon");
            }
            Stats = weapon;
        }

        public WeaponStats Stats
        {
            get; private set;
        }

        public virtual bool TargetIsValid(GameUnit attacker, ITargetable defender, Tile targetTile)
        {
            if (attacker == null || defender == null || targetTile == null)
            {
                return false;
            }

            switch (Stats.Type)
            {
                case WeaponType.Melee:
                    return MeleeTargetValid(attacker, defender, targetTile);
                case WeaponType.Ranged:
                    return RangedTargetValid(attacker, defender, targetTile);
            }

            return false;
        }

        private bool MeleeTargetValid(GameUnit attacker, ITargetable defender, Tile targetTile)
        {
            return attacker.Position.CurrentTile.IsAdjacent(targetTile);
        }

        private bool RangedTargetValid(GameUnit attacker, ITargetable defender, Tile targetTile)
        {
            var rangedStats = Stats as RangedWeaponStats;
            if (rangedStats == null)
            {
                return false;
            }

            var distance = Tile.GetDistance(attacker.Position.CurrentTile, targetTile);
            return distance <= rangedStats.Range;
        }
    }
}
