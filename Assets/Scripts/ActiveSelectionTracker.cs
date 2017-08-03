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
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, EventArgs.Empty);
                }
            }
        }

        public void SetSelection(Component comp)
        {
            SetSelection(comp.gameObject);
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
    }
}
