namespace DLS.LD39
{
    using DLS.LD39.Units;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class CameraTargeter : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        public void TargetUnit(GameObject unit)
        {
            transform.position = new Vector3(
                unit.transform.position.x,
                unit.transform.position.y,
                transform.position.z);
        }
    }
}
