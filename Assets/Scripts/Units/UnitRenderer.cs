namespace DLS.LD39.Units
{
    using UnityEngine;
    
    class UnitRenderer : MonoBehaviour
    {
        private MeshRenderer _renderer;

        public void BeginTurn()
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<MeshRenderer>();
            }
            if (_renderer != null)
            {
                SetSelected();
            }
        }

        public void EndTurn()
        {
            if (_renderer != null)
            {
                SetUnselected();
            }
        }

        private void SetSelected()
        {
            _renderer.material = MaterialRepository.Instance.SelectedCubeMaterial;
        }

        private void SetUnselected()
        {
            _renderer.material = MaterialRepository.Instance.BaseCubeMaterial;
        }
    }
}
