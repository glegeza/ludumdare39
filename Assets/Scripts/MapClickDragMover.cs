namespace DLS.LD39
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [RequireComponent(typeof(Camera))]
    class MapClickDragMover : MonoBehaviour
    {
        public float MoveSpeed = 3.0f;
        public Vector2 LowerLeftBounds = new Vector2(-40.0f, -40.0f);
        public Vector2 UpperRightBounds = new Vector2(40.0f, 40.0f);

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(2))
            {
                transform.position += new Vector3(
                    -Input.GetAxisRaw("Mouse X") * Time.deltaTime * MoveSpeed,
                    -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * MoveSpeed,
                    0.0f);

                var x = Mathf.Clamp(transform.position.x, LowerLeftBounds.x, UpperRightBounds.x);
                var y = Mathf.Clamp(transform.position.y, LowerLeftBounds.y, UpperRightBounds.y);

                transform.position = new Vector3(x, y, transform.position.z);
            }
        }
    }
}
