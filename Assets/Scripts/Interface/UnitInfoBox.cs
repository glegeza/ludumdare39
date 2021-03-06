﻿namespace DLS.LD39.Interface
{
    using Combat;
    using Units;
    using System;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.UI;

    [UsedImplicitly]
    public class UnitInfoBox : MonoBehaviour
    {
        public Text Title;
        public Text Initiative;
        public Text AP;
        public Text Speed;
        public Text Aim;
        public Text HP;
        public Text Energy;
        public Text Evasion;
        public Text ChanceToHit;

        [UsedImplicitly]
        private void Update()
        {
            var selectedObject = ActiveSelectionTracker.Instance.SelectedObject;
            if (selectedObject == null)
            {
                ClearUnit();
                return;
            }

            var selectedUnit = selectedObject.GetComponent<GameUnit>();
            if (selectedUnit == null)
            {
                ClearUnit();
                return;
            }

            UpdateUnit(selectedUnit);
        }

        private void UpdateUnit(GameUnit unit)
        {
            Title.text = unit.UnitName;
            Initiative.text = String.Format("Initiative: {0}", unit.SecondaryStats.Initiative);
            AP.text = String.Format("AP: {0}/{1}", unit.AP.PointsRemaining, unit.SecondaryStats.ActionPointCap);
            Speed.text = String.Format("Speed: {0}", unit.PrimaryStats.Speed);
            Aim.text = String.Format("Aim: {0}", unit.PrimaryStats.Aim);
            HP.text = String.Format("HP: {0}/{1}", unit.CombatInfo.HitPoints, unit.PrimaryStats.MaxHP);
            Evasion.text = String.Format("Evasion: {0}", unit.PrimaryStats.Evasion);

            var energy = unit.GetComponent<EnergyPoints>();
            if (energy != null)
            {
                Energy.text = String.Format("Energy: {0}/{1}", energy.PointsRemaining, energy.EnergyCapacity);
            }
            else
            {
                Energy.text = "";
            }

            if (unit.Faction == Faction.Player && unit.CurrentTarget != null)
            {
                WeaponStats weapon = unit.Equipment.PrimaryWeapon.SlotItem.Stats;
                if (weapon == null)
                {
                    ChanceToHit.text = "";
                }
                var hitChance = CombatManager.HitChance(
                    unit, 
                    weapon, 
                    unit.CurrentTarget.CombatInfo, 
                    unit.CurrentTarget.Position.CurrentTile);
                ChanceToHit.text = String.Format("Chance To Hit: {0}%", hitChance);
            }
            else
            {
                ChanceToHit.text = "";
            }

        }

        private void ClearUnit()
        {
            Title.text = "No unit selected";
            Initiative.text = "";
            AP.text = "";
            Speed.text = "";
            Aim.text = "";
            HP.text = "";
            Evasion.text = "";
            Energy.text = "";
            ChanceToHit.text = "";
        }
    }
}
