namespace DLS.LD39.Interface
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine.UI;
    using UnityEngine;
    using Props;

    [RequireComponent(typeof(Dropdown))]
    [UsedImplicitly]
    public class PropDropdownPopulator : MonoBehaviour
    {
        private Dropdown _dropdown;

        [UsedImplicitly]
        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.ClearOptions();

            var factory = PropFactory.Instance;
            var options = new List<string>();
            foreach (var prop in factory.PropIDs)
            {
                options.Add(prop);
            }
            _dropdown.AddOptions(options);
        }
    }
}
