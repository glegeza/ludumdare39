namespace DLS.LD39
{
    using JetBrains.Annotations;
    using Map;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class MapClickDragMover : MonoBehaviour
    {
        #pragma warning disable 0649
        public TileMap AttachedMap;
        #pragma warning restore 0649
        public float MoveSpeed = 3.0f;
        [Range(0.0f, 10.0f)]
        public float KeyboardModifier = 0.5f;
        public Vector2 LowerLeftBounds = new Vector2(-40.0f, -40.0f);
        public Vector2 UpperRightBounds = new Vector2(40.0f, 40.0f);

        [UsedImplicitly]
        private void Start()
        {
            if (AttachedMap == null) return;
            
            LowerLeftBounds = new Vector2(-(AttachedMap.Width / 2.0f),
                -(AttachedMap.Height / 2.0f));
            UpperRightBounds = new Vector2(AttachedMap.Width / 2.0f,
                AttachedMap.Height / 2.0f);
        }

        [UsedImplicitly]
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
