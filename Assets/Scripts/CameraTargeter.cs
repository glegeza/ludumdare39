namespace DLS.LD39
{
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    [UsedImplicitly]
    public class CameraTargeter : MonoBehaviour
    {
        public void TargetUnit(GameObject unit)
        {
            transform.position = new Vector3(
                unit.transform.position.x,
                unit.transform.position.y,
                transform.position.z);
        }
    }
}
