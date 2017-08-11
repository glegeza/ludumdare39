namespace DLS.LD39.MouseInput
{
    using UnityEngine;
    
    public abstract class BaseClickHandler<T> : ComponentClickHandler where T : MonoBehaviour
    {
        sealed public override bool GameObjectClicked(GameObject obj, int btn, Vector2 hitPoint)
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

        sealed public override bool GameObjectMouseDown(GameObject obj, int btn, Vector2 hitPoint)
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
            {
                if (HandleMouseDown(comp, btn, hitPoint))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool HandleClick(T comp, int btn, Vector2 hitPoint)
        {
            return false;
        }

        public virtual bool HandleMouseDown(T comp, int btn, Vector2 hitPoint)
        {
            return false;
        }
    }
}
