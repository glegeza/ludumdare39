namespace DLS.LD39.Interface
{
    using MouseInput;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    class MapClickRouterModeText : MonoBehaviour
    {
        public MapClickRouter _router;
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            var mode = _router.ActiveMode;
            var modeName = mode == null ?
                "None" : mode.Name;
            _text.text = String.Format("Current Mode: {0}", modeName);
        }
    }
}
