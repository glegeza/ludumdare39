namespace DLS.LD39.Interface
{
    using DLS.LD39.Combat;
    using DLS.LD39.Units;
    using UnityEngine;

    public class FloatingCombatTextController : SingletonComponent<FloatingCombatTextController>
    {
        public FloatingCombatText TextPrefab;
        public Canvas UICanvas;
        public float RandomRange = 0.2f;

        public void RegisterNoAP(GameUnit unit)
        {
            CreateText("Not enough AP!", unit.transform.position);
        }

        public void RegisterNoEnergy(GameUnit unit)
        {
            CreateText("Not enough energy!", unit.transform.position);
        }

        public void RegisterDamage(int amount, ITargetable target)
        {
            var transform = GetTransform(target);
            if (transform == null)
            {
                return;
            }
            CreateText(amount.ToString(), transform.position);
        }

        public void RegisterMiss(ITargetable target)
        {
            var transform = GetTransform(target);
            if (transform == null)
            {
                return;
            }
            CreateText("Miss!", transform.position);
        }

        public void CreateText(string text, Vector3 location)
        {
            location += new Vector3(Random.Range(-RandomRange, RandomRange),
                Random.Range(-RandomRange, RandomRange), 0.0f);
            Vector2 pos = Camera.main.WorldToScreenPoint(location);
            var textObj = Instantiate(TextPrefab, UICanvas.transform, false);
            textObj.transform.position = pos;
            textObj.SetText(text);
        }

        private Transform GetTransform(ITargetable target)
        {
            var mb = target as MonoBehaviour;
            return mb == null ? null : mb.transform;
        }
    }
}