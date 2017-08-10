namespace DLS.LD39.MouseInput
{
    using UnityEngine;

    public delegate void ClickDelegate<T>(T comp, int btn, Vector2 hitPoint) where T : MonoBehaviour;
}
