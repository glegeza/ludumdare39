namespace DLS.LD39.Interface
{
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;
    using DLS.LD39.Props;

    [RequireComponent(typeof(Dropdown))]
    public class PropDropdownPopulator : MonoBehaviour
    {
        private Dropdown _dropdown;

        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.ClearOptions();

            var factory = PropFactory.Instance;
            var options = new List<string>();
            foreach (var prop in factory.Props)
            {
                options.Add(prop.ID);
            }
            _dropdown.AddOptions(options);
        }
    }
}
