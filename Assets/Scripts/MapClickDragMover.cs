namespace DLS.LD39
{
    using DLS.LD39.Map;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    class MapClickDragMover : MonoBehaviour
    {
        public TileMap AttachedMap;
        public float MoveSpeed = 3.0f;
        [Range(0.0f, 10.0f)]
        public float KeyboardModifier = 0.5f;
        public Vector2 LowerLeftBounds = new Vector2(-40.0f, -40.0f);
        public Vector2 UpperRightBounds = new Vector2(40.0f, 40.0f);

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            if (AttachedMap != null)
            {
                LowerLeftBounds = new Vector2(-(AttachedMap.Width / 2.0f),
                    -(AttachedMap.Height / 2.0f));
                UpperRightBounds = new Vector2(AttachedMap.Width / 2.0f,
                    AttachedMap.Height / 2.0f);
            }
        }

        private void Update()
        {
            var x = 0.0f;
            var y = 0.0f;

            if (Input.GetMouseButton(2))
            {
                x = -Input.GetAxisRaw("Mouse X");
                y = -Input.GetAxisRaw("Mouse Y");
            }
            else
            { 
                if (Input.GetKey(KeyCode.A))
                {
                    x += -1.0f * KeyboardModifier;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    x += 1.0f * KeyboardModifier;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    y += 1.0f * KeyboardModifier;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    y += -1.0f * KeyboardModifier;
                }
            }

            
            transform.position += new Vector3(
                x * Time.deltaTime * MoveSpeed,
                y * Time.deltaTime * MoveSpeed,
                0.0f);

            var transX = Mathf.Clamp(transform.position.x, LowerLeftBounds.x, UpperRightBounds.x);
            var transY = Mathf.Clamp(transform.position.y, LowerLeftBounds.y, UpperRightBounds.y);

            transform.position = new Vector3(transX, transY, transform.position.z);
        }
    }
}
