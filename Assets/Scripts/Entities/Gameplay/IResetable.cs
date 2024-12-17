using R3;

namespace PEntities.Gameplay
{
    public interface IResettable<T>
    {
        public ReactiveCommand<T> Reset { get; }
    }
}