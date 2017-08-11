namespace DLS.LD39.Interface
{
    using UnityEngine;

    public class FloatingCombatTextController : SingletonComponent<FloatingCombatTextController>
    {
        public FloatingCombatText TextPrefab;
        public Canvas UICanvas;

        public void CreateText(string text, Vector3 location)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(location);
            var textObj = Instantiate(TextPrefab, UICanvas.transform, false);
            textObj.transform.position = pos;
            textObj.SetText(text);
        }
    }
}