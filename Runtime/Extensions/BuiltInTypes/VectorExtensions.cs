using UnityEngine;

namespace AnkleBreaker.Utils.Extensions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns a copy of this Vector3 with optionally overridden components.
        /// Usage: pos.With(y: 0f) or pos.With(x: 5f, z: 10f)
        /// </summary>
        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
        }

        /// <summary>
        /// Returns this Vector3 with Y set to 0. Useful for ground-plane projection.
        /// </summary>
        public static Vector3 Flat(this Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }

        /// <summary>
        /// Converts a Vector2 to a Vector3 on the XZ plane (Y = 0).
        /// </summary>
        public static Vector3 ToVector3XZ(this Vector2 v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        /// <summary>
        /// Returns a copy of this Vector2 with optionally overridden components.
        /// </summary>
        public static Vector2 With(this Vector2 v, float? x = null, float? y = null)
        {
            return new Vector2(x ?? v.x, y ?? v.y);
        }
    }
}
