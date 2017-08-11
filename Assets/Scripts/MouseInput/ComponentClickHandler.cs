namespace DLS.LD39.MouseInput
{
    using UnityEngine;

    public abstract class ComponentClickHandler : ScriptableObject
    {
        public virtual bool GameObjectClicked(GameObject obj, int btn, Vector2 hitPoint)
        {
            return false;
        }

        public virtual bool GameObjectMouseDown(GameObject obj, int btn, Vector2 hitPoint)
        {
            return false;
        }
    }
}
