namespace DLS.LD39.Editor
{
    using UnityEditor;
    using DLS.LD39.Units;
    using UnityEngine;

    [CustomEditor(typeof(StatsData))]
    class StatsDataInspector : Editor
    {
        private void GetMinMaxInt(string label, int curMin, int curMax, out int min, out int max)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Min", GUILayout.Width(30));
            min = EditorGUILayout.IntField(curMin, GUILayout.Width(40));
            GUILayout.Space(30);
            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
            max = EditorGUILayout.IntField(curMax, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            max = Mathf.Clamp(max, 0, StatsData.MaxStatValue);
            min = Mathf.Clamp(min, 0, max);
        }

        public override void OnInspectorGUI()
        {
            var statsData = (StatsData)target;

            var dontExpand = GUILayout.ExpandWidth(false);

            int outMin, outMax;
            GetMinMaxInt("Aim", statsData.BaseAimMin, statsData.BaseAimMax, out outMin, out outMax);
            statsData.BaseAimMin = outMin;
            statsData.BaseAimMax = outMax;

            GetMinMaxInt("Evasion", statsData.BaseEvasionMin, statsData.BaseEvasionMax, out outMin, out outMax);
            statsData.BaseEvasionMin = outMin;
            statsData.BaseEvasionMax = outMax;

            GetMinMaxInt("Armor", statsData.BaseArmorMin, statsData.BaseArmorMax, out outMin, out outMax);
            statsData.BaseArmorMin = outMin;
            statsData.BaseArmorMax = outMax;

            GetMinMaxInt("Speed", statsData.BaseSpeedMin, statsData.BaseSpeedMax, out outMin, out outMax);
            statsData.BaseSpeedMin = outMin;
            statsData.BaseSpeedMax = outMax;

            GetMinMaxInt("HP", statsData.BaseHPMin, statsData.BaseHPMax, out outMin, out outMax);
            statsData.BaseHPMin = outMin;
            statsData.BaseHPMax = outMax;

            EditorUtility.SetDirty(target);
        }
    }
}
