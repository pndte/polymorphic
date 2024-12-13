namespace PEntities.Services
{
    public interface IFactory<out TValue>
    {
        public TValue Create();
    }
}