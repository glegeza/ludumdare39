namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using UnityEngine;
    using UnityEngine.UI;

    class TileEditModeInputHandler : MapClickInputHandler
    {
        private const int PassableIdx = 7;
        private const int ImpassableIdx = 4;

        private enum Mode
        {
            DoNothing,
            Passable,
            Impassable
        }
        
        private Dropdown _selector;

        private Dropdown Selector
        {
            get
            {
                if (_selector == null)
                {
                    _selector = GameObject.FindObjectOfType<TileDropdownPopulator>().GetComponent<Dropdown>();
                }
                return _selector;
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
