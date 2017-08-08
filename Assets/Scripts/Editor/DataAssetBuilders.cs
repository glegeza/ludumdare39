namespace DLS.LD39.Editor
{
    using UnityEditor;
    using DLS.LD39.Units;
    using DLS.LD39.Map;
    using DLS.LD39.Props;
    using System;

    class DataAssetBuilders
    {
        private const string MenuPath = "Assets/Create/Game Data/";

        [MenuItem(MenuPath + "Game Unit")]
        public static void CreateUnitDataAsset()
        {
            ScriptableObjectUtility.CreateAsset<UnitData>();
        }

        [MenuItem(MenuPath + "Game Unit Stats")]
        public static void CreateStatsDataAsset()
        {
            ScriptableObjectUtility.CreateAsset<StatsData>();
        }

        [MenuItem(MenuPath + "Tile Set")]
        public static void CreateTileSetDataAsset()
        {
            ScriptableObjectUtility.CreateAsset<TileSetData>();
        }

        [MenuItem(MenuPath + "Prop")]
        public static void CreatePropDataAsset()
        {
            ScriptableObjectUtility.CreateAsset<PropData>();
        }
    }
}
