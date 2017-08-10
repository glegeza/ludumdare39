namespace DLS.LD39
{
    using UnityEngine;

    public class ObjectTypeClickCallback<T> : IComponentClickHandler where T : MonoBehaviour
    {
        private ClickDelegate<T> _delegate;

        public ObjectTypeClickCallback(ClickDelegate<T> del)
        {
            _delegate = del;
        }

        public bool CheckForComponent(GameObject obj, int btn, Vector2 hitPoint)
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
            {
                _delegate(comp, btn, hitPoint);
                return true;
            }

            return false;
        }
    }
}
