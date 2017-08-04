namespace DLS.LD39.Editor
{
    using DLS.LD39.Map;
    using UnityEditor;

    public class TileSetAsset
    {
        [MenuItem("Assets/Create/Tile Set")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<TileSetData>();
        }
    }
}
