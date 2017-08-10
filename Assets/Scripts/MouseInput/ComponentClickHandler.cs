namespace DLS.LD39.MouseInput
{
    using UnityEngine;

    public abstract class ComponentClickHandler : ScriptableObject
    {
        public abstract bool CheckForComponent(GameObject obj, int btn, Vector2 hitPoint);
    }
}
