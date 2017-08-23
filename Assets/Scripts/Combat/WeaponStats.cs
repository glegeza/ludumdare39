using DLS.LD39.Actions;
using DLS.LD39.Equipment.Data;
using DLS.LD39.Map;
using System.Collections.Generic;
using UnityEngine;

namespace DLS.LD39.Combat
{
    public abstract class WeaponStats
    {
        protected WeaponStats(WeaponData data, WeaponType type)
        {
            APCost = data.APCost;
            EnergyCost = data.EnergyCost;
            MinDamage = data.MinDamage;
            MaxDamage = data.MaxDamage;
            BaseToHit = data.BaseToHitModifier;
            Type = type;
            Slot = data.Slot;
            SpriteIcon = data.Icon;
            Actions = new List<Action>(data.Actions);
        }

        public int APCost
        {
            get; private set;
        }

        public int EnergyCost
        {
            get; private set;
        }

        public int MinDamage
        {
            get; private set;
        }

        public int MaxDamage
        {
            get; private set;
        }

        public int BaseToHit
        {
            get; private set;
        }

        public WeaponType Type
        {
            get; private set;
        }

        public WeaponSlot Slot
        {
            get; private set;
        }

        public Sprite SpriteIcon
        {
            get; private set;
        }

        public List<Action> Actions
        {
            get; private set;
        }

        public abstract bool TileIsLegalTarget(Tile attackerPos, Tile targetPos);
    }
}
