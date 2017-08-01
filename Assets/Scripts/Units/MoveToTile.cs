namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Pathfinding;

    [RequireComponent(typeof(TilePosition))]
    public class MoveToTile : MonoBehaviour
    {
        public float MoveTimer = 1.0f;

        private static Dictionary<KeyCode, TileEdge> _keyMoveMap = new Dictionary<KeyCode, TileEdge>()
        {
            { KeyCode.UpArrow, TileEdge.Up },
            { KeyCode.DownArrow, TileEdge.Down },
            { KeyCode.LeftArrow, TileEdge.Left },
            { KeyCode.RightArrow, TileEdge.Right },
        };
        private TilePicker _picker;
        private TilePosition _position;
        private Queue<Tile> _path = new Queue<Tile>();
        private float _timeSinceLastMove = 0.0f;
        private SimplePathfinder _pathFinder = new SimplePathfinder();

        private void Start()
        {
            var tileMap = FindObjectOfType<TileMap>();
            _picker = new TilePicker(tileMap);
            _position = GetComponent<TilePosition>();
            _position.SetTile(tileMap.GetTile(0, 0));
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var targetTile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                if (targetTile != null)
                {
                    SetNewPath(targetTile);
                }
            }

            if (!_path.Any())
            {
                return;
            }

            _timeSinceLastMove += Time.deltaTime;
            if (_timeSinceLastMove > MoveTimer)
            {
                TakeStep();
            }
        }

        private void SetNewPath(Tile target)
        {
            _path.Clear();
            _path = _pathFinder.GetPath(_position.CurrentTile, target);
        }

        private void TakeStep()
        {
            var nextMove = _path.Dequeue();
            if (TileIsValid(nextMove))
            {
                _position.SetTile(nextMove);
                _timeSinceLastMove = 0.0f;
            }
            else
            {
                _path.Clear();
                _timeSinceLastMove = 0.0f;
                Debug.LogError("Invalid move attempted.");
            }
        }

        private bool TileIsValid(Tile target)
        {
            return Mathf.Abs(target.X - _position.CurrentTile.X) <= 1 &&
                Mathf.Abs(target.Y - _position.CurrentTile.Y) <= 1 &&
                target.Passable;
        }
    }
}
