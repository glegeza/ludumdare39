namespace DLS.LD39.Actions
{
    using System.Collections;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Actions/Fast Recharge")]
    public class FastRecharge : Action
    {
        public int APCost;
        public int EnergyRecharge;

        public override ActionSelectMode Mode
        {
            get
            {
                return ActionSelectMode.None;
            }
        }

        public override bool ActionIsValid(GameUnit actor, GameObject target, Tile targetTile)
        {
            return actor != null && actor.Equipment.Battery.SlotItem != null &&
                actor.GetComponent<EnergyPoints>() != null;
        }

        public override bool AttemptAction(GameUnit actor, GameObject target, Tile targetTile, ActionCompletedDelegate onActionCompletedDelegate)
        {
            if (!ActionIsValid(actor, target, targetTile))
            {
                return false;
            }

            var energyPoints = actor.GetComponent<EnergyPoints>();

            energyPoints.AddPoints(EnergyRecharge);

            actor.StartCoroutine(WaitOneFrame(onActionCompletedDelegate));

            return true;
        }

        public override int GetAPCost(GameUnit unit)
        {
            return APCost;
        }

        public override string GetDescription(GameUnit unit)
        {
            return "Recharge dat battery";
        }

        public override int GetEnergyCost(GameUnit unit)
        {
            return 0;
        }

        public override string GetName(GameUnit unit)
        {
            return "Recharge";
        }

        public override Sprite GetSprite(GameUnit unit)
        {
            return null;
        }

        public IEnumerator WaitOneFrame(ActionCompletedDelegate cb)
        {
            yield return new WaitForEndOfFrame();
            cb();
        }
    }
}
