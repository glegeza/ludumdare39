namespace DLS.Utility
{
    /// <summary>
    /// Builds simple hash codes based on Josh Bloch's Effective Java algo.
    /// </summary>
    public static class SimpleHashBuilder
    {
        public const int Seed = 7;
        public const int Multiplier = 17;

        public static int GetHash(params object[] fields)
        {
            unchecked
            {
                int hash = Seed;
                foreach (var field in fields)
                {
                    hash = hash * Multiplier + field.GetHashCode();
                }

                return hash;
            }
        }
    }
}
