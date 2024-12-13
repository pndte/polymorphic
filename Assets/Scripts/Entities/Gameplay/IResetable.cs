using System;

namespace PEntities.Gameplay
{
    public interface IResettable<out T>
    {
        public void Reset();
        public event Action<T> OnReset;
    }
}