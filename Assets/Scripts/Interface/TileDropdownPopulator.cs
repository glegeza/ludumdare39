namespace DLS.LD39.Interface
{
    using Map;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Dropdown))]
    [UsedImplicitly]
    public class TileDropdownPopulator : MonoBehaviour
    {
        private Dropdown _dropdown;

        [UsedImplicitly]
        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.ClearOptions();

            var tilemap = FindObjectOfType<TileMap>();
            var options = new List<string>();
            foreach (var tile in tilemap.TileData.TileTypes)
            {
                options.Add(tile.ID);
            }
            _dropdown.AddOptions(options);
        }
    }
}
