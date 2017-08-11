namespace DLS.LD39.AI.Actions
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/Do Nothing")]
    class DoNothing : AIAction
    {
        public override bool Act(StateController controller)
        {
            return false;
        }
    }
}
