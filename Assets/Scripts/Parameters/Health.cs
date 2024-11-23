using R3;
using UnityEngine;

namespace Parameters
{
    public class Health: MonoBehaviour
    {
        public SerializableReactiveProperty<int> Current;
        public SerializableReactiveProperty<int> Maximum;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void Start()
        {
            Init(); // TODO: Load Pool
        }

        public void Init()
        {
            SubscribeAll();
        }

        private void SubscribeAll()
        {
            ValidateZero(Maximum);
            ValidateZero(Current);
            
            Current.Where(currentValue => currentValue > Maximum.Value)
                .Subscribe(_ => Current.Value = Maximum.Value)
                .AddTo(_disposable);
            Maximum.Where(maximumValue => Current.Value > maximumValue)
                .Subscribe(_ => Current.Value = Maximum.Value)
                .AddTo(_disposable);
            
            Current.Where(currentValue => currentValue == 0)
                .Subscribe(_ => Die())
                .AddTo(_disposable);
        }

        private void ValidateZero(ReactiveProperty<int> reactiveProperty)
        {
            reactiveProperty.Where(value => value < 0)
                .Subscribe(_ => reactiveProperty.Value = 0)
                .AddTo(_disposable);
        }
        
        private void Die() => print("Dead");

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}