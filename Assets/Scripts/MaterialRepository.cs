namespace DLS.LD39
{
    using JetBrains.Annotations;
    using UnityEngine;

    // UGLY this is super lazy and ugly, but IDGAF
    /// <summary>
    /// Stores references to useful materials.
    /// </summary>
    public class MaterialRepository : MonoBehaviour
    {
        private static MaterialRepository _instance;

        public static MaterialRepository Instance
        {
            get
            {
                return _instance;
            }
        }

        #pragma warning disable 0649
        public Material BaseCubeMaterial;
        public Material SelectedCubeMaterial;
        public Material PathMarkerMaterial;
        public Material FuturePathMarkerMaterial;
        #pragma warning restore 0649
        
        [UsedImplicitly]
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
