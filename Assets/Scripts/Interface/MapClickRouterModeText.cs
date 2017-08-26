namespace DLS.LD39.Interface
{
    using MouseInput;
    using System;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    public class MapClickRouterModeText : MonoBehaviour
    {
        #pragma warning disable 0649
        public MapClickRouter Router = null;
        #pragma warning restore 0649
        private Text _text;

        [UsedImplicitly]
        private void Start()
        {
            _text = GetComponent<Text>();
        }

        [UsedImplicitly]
        private void Update()
        {
            var mode = Router.ActiveMode;
            var modeName = mode == null ?
                "None" : mode.Name;
            _text.text = String.Format("Current Mode: {0}", modeName);
        }
    }
}
