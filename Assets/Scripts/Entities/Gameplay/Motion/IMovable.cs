using UnityEngine;

namespace Entities.Gameplay.Motion
{
    public interface IMovable
    {
        public void Move(Vector2 direction);
    }
}