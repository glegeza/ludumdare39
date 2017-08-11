namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using DLS.LD39.Props;
    using UnityEngine;
    using UnityEngine.UI;

    class PropEditModeInputHandler : MapClickInputHandler
    {
        private enum Mode
        {
            DoNothing,
            PlaceProp,
            RemoveProp
        }

        private Dropdown _selector;
        private Mode _currentMode = Mode.DoNothing;

        private Dropdown Selector
        {
            get
            {
                if (_selector == null)
                {
                    _selector = GameObject.FindObjectOfType<PropDropdownPopulator>().GetComponent<Dropdown>();
                }
                return _selector;
            }
        }

        public PropEditModeInputHandler() : base("prop", "Prop Placement")
        { }

        public override bool HandleButtonDown(int button, Tile clickedTile)
        {
            var id = Selector.options[Selector.value].text;
            var propData = PropFactory.Instance.GetPropByID(id);
            if (propData == null)
            {
                return false;
            }
            var layer = propData.Layer;

            if (button != 0 || _currentMode == Mode.DoNothing)
            {
                return false;
            }

            var currentProp = clickedTile.PropOnLayer(layer);

            if (_currentMode == Mode.PlaceProp && currentProp == null)
            {
                PropFactory.Instance.BuildPropAndAddToTile(id, clickedTile);
            }
            else if (_currentMode == Mode.RemoveProp && currentProp != null)
            {
                clickedTile.RemoveProp(layer);
                GameObject.Destroy(currentProp.gameObject);
            }
            
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
        {
            var id = Selector.options[Selector.value].text;
            var propData = PropFactory.Instance.GetPropByID(id);
            if (propData == null)
            {
                return false;
            }
            var layer = propData.Layer;

            if (button != 0 || clickedTile == null)
            {
                return false;
            }

            var prop = clickedTile.PropOnLayer(layer);

            _currentMode = prop == null ? Mode.PlaceProp : Mode.RemoveProp;

            return false;
        }
    }
}
