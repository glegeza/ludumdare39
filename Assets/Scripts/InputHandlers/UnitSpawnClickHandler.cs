namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using UnityEngine;
    using UnityEngine.UI;

    class UnitSpawnClickHandler : MapClickInputHandler
    {
        private Dropdown _selector;

        public UnitSpawnClickHandler() : base("spawn", "Unit Spawning")
        {
            var selectorObj = GameObject.FindObjectOfType<SpawnDropdownPopulator>();
            _selector = selectorObj.GetComponent<Dropdown>();
        }

        public override bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
        {
            var id = _selector.options[_selector.value].text;
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
