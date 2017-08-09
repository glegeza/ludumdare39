namespace DLS.LD39.Units
{
    /// <summary>
    /// Component used to indicate that an object is part of the regular turn
    /// order.
    /// </summary>
    public class Initiative : GameUnitComponent
    { 

        public float InitiativeValue
        {
            get
            {
                return AttachedUnit.Stats.Speed;
            }
        }
    }
}
