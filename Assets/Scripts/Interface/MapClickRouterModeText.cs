namespace DLS.LD39.Interface
{
    using DLS.LD39.Controllers;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    class MapClickRouterModeText : MonoBehaviour
    {
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            var mode = MapClickRouter.Instance.ActiveMode;
            var modeName = mode == null ?
                "None" : mode.Name;
            _text.text = String.Format("Current Mode: {0}", modeName);
        }
    }
}
