namespace DLS.LD39.MouseInput
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    public class MouseInputRouter : MonoBehaviour
    {
        public List<ComponentClickHandler> ClickHandlers = new List<ComponentClickHandler>();
        public LayerMask ClickLayer;

        [UsedImplicitly]
        private void Update()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            var position = Input.mousePosition;

            for (var i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    var hit = GetColliderHit(position);
                    if (hit.collider != null)
                    {
                        HandleClick(i, hit.collider.gameObject, hit.point);
                    }
                }
                if (Input.GetMouseButton(i))
                {
                    var hit = GetColliderHit(position);
                    if (hit.collider != null)
                    {
                        HandleMouseDown(i, hit.collider.gameObject, hit.point);
                    }
                }
            }
        }

        private RaycastHit2D GetColliderHit(Vector3 position)
        {
            var ray = Camera.main.ScreenToWorldPoint(position);
            return Physics2D.Raycast(ray, Vector2.zero, 10000, ClickLayer.value);
        }

        private void HandleClick(int btn, GameObject obj, Vector2 hitPoint)
        {
            foreach (var handler in ClickHandlers)
            {
                if (handler.GameObjectClicked(obj, btn, hitPoint))
                {
                    break;
                }
            }
        }

        private void HandleMouseDown(int btn, GameObject obj, Vector2 hitPoint)
        {
            foreach (var handler in ClickHandlers)
            {
                if (handler.GameObjectMouseDown(obj, btn, hitPoint))
                {
                    break;
                }
            }
        }
    }
}
