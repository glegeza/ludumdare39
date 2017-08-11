namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using UnityEngine;
    using UnityEngine.UI;

    class UnitSpawnModeInputHandler : MapClickInputHandler
    {
        private Dropdown _selector;

        public UnitSpawnModeInputHandler() : base("spawn", "Unit Spawning")
        { }

        private Dropdown Selector
        {
            get
            {
                if (_selector == null)
                {
                    _selector = GameObject.FindObjectOfType<SpawnDropdownPopulator>().GetComponent<Dropdown>();
                }
                return _selector;
            }
        }

        public override bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
        {
            var id = Selector.options[Selector.value].text;
            if (button == 0)
            {
                UnitSpawner.Instance.SpawnUnit(id, clickedTile);
            }
            else if (button == 1)
            {
                var unit = ActiveUnits.Instance.GetUnitAtTile(clickedTile);
                if (unit != null)
                {
                    ActiveUnits.Instance.RemoveUnit(unit);
                }
            }
            return false;
        }
    }
}
