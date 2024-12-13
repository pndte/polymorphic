using Cysharp.Threading.Tasks;

namespace PEntities.Gameplay.Combat
{
    public interface IReloadable
    {
        public void Reload();
        public UniTask ReloadAsync();
        public bool Reloaded { get; }
    }
}