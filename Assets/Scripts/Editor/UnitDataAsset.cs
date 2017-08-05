namespace DLS.LD39.Editor
{
    using UnityEditor;
    using DLS.LD39.Units;

    public class UnitDataAsset
    {
        [MenuItem("Assets/Create/Unit Data")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<UnitData>();
        }
    }
}
