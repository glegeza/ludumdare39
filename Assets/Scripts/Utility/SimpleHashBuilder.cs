// ReSharper disable once CheckNamespace
namespace DLS.Utility
{
    using System.Linq;

    /// <summary>
    /// Builds simple hash codes based on Josh Bloch's Effective Java algo.
    /// </summary>
    public static class SimpleHashBuilder
    {
        private const int Seed = 7;
        private const int Multiplier = 17;

        public static int GetHash(params object[] fields)
        {
            unchecked
            {
                return fields.Aggregate(Seed, (current, field) => current * Multiplier + field.GetHashCode());
            }
        }
    }
}