using UnityEngine;

namespace PEntities
{
    public static class Vector2Extensions
    {
        public static Vector2 Truncate(this Vector2 vector, float maxLength)
        {
            if (vector.magnitude > maxLength)
            {
                return vector.normalized * maxLength;
            }
            return vector;
        }
    }
}