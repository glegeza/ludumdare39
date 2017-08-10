namespace DLS.LD39.MouseInput
{
    using UnityEngine;
    
    public abstract class BaseClickHandler<T> : ComponentClickHandler where T : MonoBehaviour
    {
        public override bool CheckForComponent(GameObject obj, int btn, Vector2 hitPoint)
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
            {
                if (HandleClick(comp, btn, hitPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public abstract bool HandleClick(T comp, int btn, Vector2 hitPoint);
    }
}
