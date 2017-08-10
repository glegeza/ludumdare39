namespace DLS.LD39.MouseInput
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MouseInputRouter : MonoBehaviour
    {
        public List<ComponentClickHandler> ClickHandlers = new List<ComponentClickHandler>();
        public LayerMask ClickLayer;

        private void Update()
        {
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
                if (handler.CheckForComponent(obj, btn, hitPoint))
                {
                    break;
                }
            }
        }
    }
}
