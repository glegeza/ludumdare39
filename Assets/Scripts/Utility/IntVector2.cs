namespace DLS.Utility
{
    using UnityEngine;
    using System;

    [Serializable]
    public class IntVector2 : IEquatable<IntVector2>
    {
        private int _x, _y;

        public IntVector2(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public IntVector2(float x, float y)
        {
            _x = (int)x;
            _y = (int)y;
        }

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        public Vector2 AsVector2()
        {
            return new Vector2(_x, _y);
        }

        public Vector3 AsVector3()
        {
            return new Vector3(_x, _y, 0);
        }

        public override int GetHashCode()
        {
            return SimpleHashBuilder.GetHash(_x, _y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals(obj as IntVector2);
        }

        public bool Equals(IntVector2 other)
        {
            return other != null &&
                other._x == _x &&
                other._y == _y;
        }

        public override string ToString()
        {
            return "IntVector2: (X: " + _x + ", Y: " + _y + ")";
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a._x + b._x, a._y + b._y);
        }

        public static IntVector2 operator -(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a._x - b._x, a._y - b._y);
        }
    }
}