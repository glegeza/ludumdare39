namespace DLS.LD39
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    // UGLY this is super lazy and ugly, but IDGAF
    /// <summary>
    /// Stores references to useful materials.
    /// </summary>
    class MaterialRepository : MonoBehaviour
    {
        private static MaterialRepository _instance;

        public static MaterialRepository Instance
        {
            get
            {
                return _instance;
            }
        }

        public Material BaseCubeMaterial;
        public Material SelectedCubeMaterial;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}
