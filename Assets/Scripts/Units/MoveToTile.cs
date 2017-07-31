namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

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
            const int MaxSteps = 1000; // something has gone super wrong if it takes 1000 steps to get anywhere
            _timeSinceLastMove = 0.0f;

            _path.Clear();
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (_position.CurrentTile.Equals(target))
            {
                return;
            }

            Tile currentStep = _position.CurrentTile;
            var steps = 0;
            while (steps < MaxSteps)
            {
                currentStep = GetNextStep(currentStep, target);
                _path.Enqueue(currentStep);
                if (currentStep.Equals(target))
                {
                    break;
                }
                steps++;
            }
        }

        // HACK this is terrible and won't work at all if the map isn't actually a square or if shit is in the way or whatever
        private Tile GetNextStep(Tile currentTile, Tile targetTile)
        {
            Tile next = null;
            if (targetTile.X < currentTile.X)
            {
                next = currentTile.GetAdjacent(TileEdge.Left);
            }
            else if (targetTile.X > currentTile.X)
            {
                next = currentTile.GetAdjacent(TileEdge.Right);
            }
            else if (targetTile.Y > currentTile.Y)
            {
                next = currentTile.GetAdjacent(TileEdge.Up);
            }
            else if (targetTile.Y < currentTile.Y)
            {
                next = currentTile.GetAdjacent(TileEdge.Down);
            }
            return next;
        }

        private void TakeStep()
        {
            var nextMove = _path.Dequeue();
            _position.SetTile(nextMove);
            _timeSinceLastMove = 0.0f;
        }
    }
}
