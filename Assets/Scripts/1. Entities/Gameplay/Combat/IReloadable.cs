using System.Threading;
using Cysharp.Threading.Tasks;

namespace PEntities.Gameplay.Combat
{
    public interface IReloadable
    {
        public void Reload();
        public UniTask ReloadAsync(CancellationToken token);
        public bool Reloaded { get; }
    }
}