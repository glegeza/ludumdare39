namespace DLS.LD39.Interface
{
    using DLS.LD39.Map;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Dropdown))]
    class TileDropdownPopulator : MonoBehaviour
    {
        private Dropdown _dropdown;

        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.ClearOptions();

            var tilemap = FindObjectOfType<TileMap>();
            var options = new List<string>();
            foreach (var tile in tilemap.TileData.TileNames)
            {
                options.Add(tile);
            }
            _dropdown.AddOptions(options);
        }
    }
}
