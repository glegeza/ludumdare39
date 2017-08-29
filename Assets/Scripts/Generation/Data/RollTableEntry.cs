namespace DLS.LD39.Generation.Data
{
    using System;

    [Serializable]
    public abstract class RollTableEntry<T>
    {
        public float Probability;
        public T Item;
    }
}
