namespace DLS.LD39
{
    using System;
    using UnityEngine;

    class ActiveSelectionTracker : MonoBehaviour
    {
        private static ActiveSelectionTracker _instance;

        public static ActiveSelectionTracker Instance
        {
            get { return _instance; }
        }

        public event EventHandler<EventArgs> SelectionChanged;

        public GameObject SelectedObject
        {
            get; private set;
        }

        public void SetSelection(GameObject obj)
        {
            if (obj != SelectedObject)
            {
                SelectedObject = obj;
                RaiseEvent(SelectionChanged, this, EventArgs.Empty);
                var targeter = FindObjectOfType<CameraTargeter>();
                targeter.TargetUnit(obj);
            }
        }

        public void SetSelection(Component comp)
        {
            SetSelection(comp.gameObject);
        }

        public void ClearSelection()
        {
            if (SelectedObject == null)
            {
                return;
            }
            SelectedObject = null;
            RaiseEvent(SelectionChanged, this, EventArgs.Empty);

        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void RaiseEvent(EventHandler<EventArgs> handler, object s, EventArgs e)
        {
            if (handler != null)
            {
                handler(s, e);
            }
        }
    }
}
