using PEntities.Meta.Data;

namespace PEntities.Gameplay.Combat
{
    public interface IBulletProvider
    {
        public IBullet Get();
        public IBullet Get(BaseBulletData bulletData);
    }
}