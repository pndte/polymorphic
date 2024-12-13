using R3;

namespace PEntities.Gameplay.Parameters
{
    public interface IHealth
    {
        public ReactiveProperty<int> Current { get; }
        public ReactiveProperty<int> Maximum { get; }
        public ReactiveProperty<bool> IsDead { get; }
    }
}