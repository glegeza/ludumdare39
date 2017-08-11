namespace DLS.LD39.Interface
{
    using UnityEngine;

    public class FloatingCombatTextController : SingletonComponent<FloatingCombatTextController>
    {
        public FloatingCombatText TextPrefab;
        public Canvas UICanvas;
        public float RandomRange = 0.2f;

        public void CreateText(string text, Vector3 location)
        {
            location += new Vector3(Random.Range(-RandomRange, RandomRange),
                Random.Range(-RandomRange, RandomRange), 0.0f);
            Vector2 pos = Camera.main.WorldToScreenPoint(location);
            var textObj = Instantiate(TextPrefab, UICanvas.transform, false);
            textObj.transform.position = pos;
            textObj.SetText(text);
        }
    }
}