namespace DLS.LD39.Interface
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Dropdown))]
    class SpawnDropdownPopulator : MonoBehaviour
    {
        private Dropdown _dropdown;

        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            var spawner = UnitSpawner.Instance;
            _dropdown.ClearOptions();
            var options = new List<string>();
            foreach (var item in spawner.UnitTypeIDs)
            {
                options.Add(item);
            }
            _dropdown.AddOptions(options);
        }
    }
}
