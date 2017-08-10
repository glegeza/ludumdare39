namespace DLS.LD39
{
    using UnityEngine;

    public interface IComponentClickHandler
    {
        bool CheckForComponent(GameObject obj, int btn, Vector2 hitPoint);
    }
}
