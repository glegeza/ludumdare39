namespace DLS.LD39.Editor
{
    using DLS.LD39.Props;
    using UnityEditor;

    public class PropDataAsset
    {
        [MenuItem("Assets/Create/Prop")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<PropData>();
        }
    }
}
