using R3;

namespace PEntities.Gameplay.Combat
{
    public interface IMortal : IDamageable
    {
        public ReactiveProperty<float> CurrentHealth { get; }
        public ReactiveProperty<float> MaximumHealth { get; }
        public ReactiveProperty<bool> IsDead { get; }
    }
}