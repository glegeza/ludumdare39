﻿namespace DLS.LD39.Interface
{
    using DLS.LD39.Units;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitInfoBox : MonoBehaviour
    {
        public Text Title;
        public Text Initiative;
        public Text AP;
        public Text Speed;
        public Text Aim;
        public Text HP;
        public Text Evasion;

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
            Title.text = unit.Name;
            Initiative.text = String.Format("Initiative: {0}", unit.SecondaryStats.Initiative);
            AP.text = String.Format("AP: {0}/{1}", unit.AP.PointsRemaining, unit.SecondaryStats.ActionPointCap);
            Speed.text = String.Format("Speed: {0}", unit.Stats.Speed);
            Aim.text = String.Format("Aim: {0}", unit.Stats.Aim);
            HP.text = String.Format("HP: {0}/{1}", unit.CombatInfo.HitPoints, unit.Stats.MaxHP);
            Evasion.text = String.Format("Evasion: {0}", unit.Stats.Evasion);
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
        }
    }
}