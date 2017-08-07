namespace DLS.LD39.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DLS.LD39.Units;
    using UnityEditor;

    class StatsDataAsset
    {
        [MenuItem("Assets/Create/Stats")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<StatsData>();
        }
    }
}
