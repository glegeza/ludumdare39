namespace DLS.LD39.Controllers
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;

    class UnitClickMover : IMapClickInputHandler
    {
        private TilePicker _picker;
        private MoveToTile _mover;

        public bool HandleTileClick(int button, Tile clickedTile)
        {
            var activeUnit = TurnManager.Instance.ActiveUnit;
            if (activeUnit == null)
            {
                return false;
            }
            
            activeUnit.GetComponent<MoveToTile>().SetNewTarget(clickedTile);

            return false;
        }
    }
}
