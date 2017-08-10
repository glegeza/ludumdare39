namespace DLS.LD39.MouseInput
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    public class UnitSelector
    {
        public bool SelectUnit(GameUnit g, int btn, Vector2 hp)
        {
            if (btn != 0)
            {
                return false;
            }

            ActiveSelectionTracker.Instance.SetSelection(g);

            return true;
        }
    }
}
