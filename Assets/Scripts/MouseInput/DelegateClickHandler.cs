namespace DLS.LD39.MouseInput
{
    using System;
    using UnityEngine;

    public class DelegateClickHandler<T> : BaseClickHandler<T> where T : MonoBehaviour
    {
        private ClickDelegate<T> _delegate;

        public DelegateClickHandler(ClickDelegate<T> del)
        {
            if (del == null)
            {
                throw new ArgumentNullException("del");
            }
            _delegate = del;
        }

        public override bool HandleClick(T comp, int btn, Vector2 hitPoint)
        {
            return _delegate(comp, btn, hitPoint);
        }
    }
}
