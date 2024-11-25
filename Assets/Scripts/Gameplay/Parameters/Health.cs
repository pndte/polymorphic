using R3;
using UnityEngine;

namespace Parameters
{
    public class Health: MonoBehaviour, IHealth
    {
        [SerializeField] private SerializableReactiveProperty<int> _current;
        [SerializeField] private SerializableReactiveProperty<int> _maximum;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void Start()
        {
            Init(); // TODO: Load Pool
        }

        public void Init()
        {
            SubscribeAll();
        }
        
        public ReactiveProperty<int> Current => _current;
        public ReactiveProperty<int> Maximum => _maximum;
        public ReactiveProperty<bool> IsDead { get; } = new ReactiveProperty<bool>(false);

        private void SubscribeAll()
        {
            ValidateZero(_maximum);
            ValidateZero(_current);
            
            _current.Where(currentValue => currentValue > _maximum.Value)
                .Subscribe(_ => _current.Value = _maximum.Value)
                .AddTo(_disposable);
            _maximum.Where(maximumValue => _current.Value > maximumValue)
                .Subscribe(_ => _current.Value = _maximum.Value)
                .AddTo(_disposable);

            _current.Subscribe(currentValue => IsDead.Value = currentValue == 0)
                .AddTo(_disposable);
            
            IsDead.Where(isDead => isDead)
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