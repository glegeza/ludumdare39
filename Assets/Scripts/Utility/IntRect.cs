﻿// ReSharper disable CheckNamespace
namespace DLS.Utility
{
    using System;
    using UnityEngine;

    public class IntRect : IEquatable<IntRect>
    {
        private readonly int _x1;
        private readonly int _y1;
        private readonly int _x2;
        private readonly int _y2;
        private readonly int _w;
        private readonly int _h;

        private readonly IntVector2 _center;
        private readonly IntVector2 _botLeft;
        private readonly IntVector2 _topRight;
        private readonly IntVector2 _topLeft;
        private readonly IntVector2 _botRight;

        public IntRect(IntVector2 botLeft, int w, int h)
            : this(botLeft.X, botLeft.Y, w, h)
        {

        }

        public IntRect(int x, int y, int w, int h)
        {
            _x1 = x;
            _y1 = y;
            _w = w;
            _h = h;
            _x2 = _x1 + _w;
            _y2 = _y1 + _h;

            _botLeft = new IntVector2(_x1, _y1);
            _botRight = new IntVector2(_x2, _y1);
            _topRight = new IntVector2(_x2, _y2);
            _topLeft = new IntVector2(_x1, _y2);
            _center = new IntVector2(_x1 + Mathf.FloorToInt(_w / 2.0f), _y1 + Mathf.FloorToInt(_h / 2.0f));
        }

        public int X
        {
            get { return _x1; }
        }

        public int Y
        {
            get { return _y1; }
        }

        public int Width
        {
            get { return _w; }
        }

        public int Height
        {
            get { return _h; }
        }

        public int Left
        {
            get { return _x1; }
        }

        public int Right
        {
            get { return _x2; }
        }

        public int Top
        {
            get { return _y2; }
        }

        public int Bottom
        {
            get { return _y1; }
        }

        public IntVector2 Center
        {
            get { return _center; }
        }

        public IntVector2 TopLeft
        {
            get { return _topLeft; }
        }

        public IntVector2 TopRight
        {
            get { return _topRight; }
        }

        public IntVector2 BottomLeft
        {
            get { return _botLeft; }
        }

        public IntVector2 BottomRight
        {
            get { return _botRight; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals(obj as IntRect);
        }

        public override int GetHashCode()
        {
            return SimpleHashBuilder.GetHash(_x1, _y1, _w, _h);
        }

        public override string ToString()
        {
            return String.Format("RectInt: [{0}, {1}] [{2}x{3}]", _x1, _y1, _w, _h);
        }

        public bool Equals(IntRect other)
        {
            return other != null &&
                   _x1 == other._x1 &&
                   _y1 == other._y1 &&
                   _w == other._w &&
                   _h == other._h;
        }

        public bool Contains(IntVector2 point)
        {
            return (point.X <= _x2 && point.X >= _x1 &&
                    point.Y <= _y2 && point.Y >= _y1);
        }

        public bool Intersects(IntRect other)
        {
            return (_x1 < other._x2 && _x2 > other._x1 &&
                    _y1 < other._y2 && _y2 > other._y1);
        }
    }
}