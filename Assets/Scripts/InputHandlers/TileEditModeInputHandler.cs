namespace DLS.LD39.InputHandlers
{
    using Interface;
    using Map;
    using UnityEngine;
    using UnityEngine.UI;

    public class TileEditModeInputHandler : MapClickInputHandler
    {
        private Dropdown _selector;

        private Dropdown Selector
        {
            get
            {
                return _selector ?? (_selector = Object.FindObjectOfType<TileDropdownPopulator>()
                           .GetComponent<Dropdown>());
            }
        }

        public TileEditModeInputHandler() : base("edit", "Map Editing")
        { }

        public override bool HandleButtonDown(int button, Tile targetTile)
        {
            if (button != 0)
            {
                return false;
            }
            var id = Selector.options[Selector.value].text;
            
            if (targetTile.Type.ID == id)
            {
                return false;
            }

            targetTile.Map.SetTileAt(targetTile.X, targetTile.Y, id);

            return false;
        }

        public override bool HandleTileClick(int button, Tile targetTile)
        {
            return false;
        }
    }
}
