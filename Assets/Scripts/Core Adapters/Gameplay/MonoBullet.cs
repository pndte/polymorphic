using System;
using Cysharp.Threading.Tasks;
using PEntities.Gameplay;
using PEntities.Gameplay.Combat;
using PEntities.Meta.Data;
using R3;
using UnityEngine;
using Zenject;

namespace PCoreAdapters.Gameplay
{
    public class MonoBullet: MonoBehaviour, IBullet, IResettable<MonoBullet>
    {
        private IBullet _bullet;
        private CompositeDisposable _disposables;
        private Rigidbody2D _physics;
        private BaseBulletData _defaultData;
        
        [Inject]
        private void Construct(IBullet bullet)
        {
            _bullet = bullet;
            Reset = new ReactiveCommand<MonoBullet>();
            _disposables = new CompositeDisposable();
            _physics = GetComponent<Rigidbody2D>();
            _defaultData =
                Resources.Load<BaseBulletConfigHolder>("Data/Combat/DefaultBulletConfig")
                    .BulletData; 
            
            Reset.Subscribe(_ => OnReset())
                .AddTo(_disposables);
        }

        private void FixedUpdate()
        {
            if (IsLaunched.Value)
                Move(LaunchedDirection.Value);
        }

        public ReactiveCommand<MonoBullet> Reset { get; private set; }
        public void Move(Vector2 direction) => _bullet.Move(direction);

        public BaseBulletData Data
        {
            get => _bullet.Data;
            set => _bullet.Data = value;
        }
        public ReactiveProperty<bool> IsLaunched => _bullet.IsLaunched;
        public ReactiveProperty<Vector2> LaunchedDirection => _bullet.LaunchedDirection;
        public void Launch(Vector2 launchOrigin, Vector2 direction) 
        {
            _bullet.Launch(launchOrigin, direction);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            ResetAsync();
        }

        private async UniTask ResetAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Data.LiveTime.Value));
            Reset.Execute(this);
        }

        private void OnReset()
        {
            IsLaunched.Value = false;
            LaunchedDirection.Value = Vector2.zero;
            transform.position = new Vector3(999, 999, 0);
            transform.rotation = Quaternion.identity;
            _physics.velocity = Vector3.zero;
            Data = _defaultData;
        }
        
        public class Factory : PlaceholderFactory<MonoBullet>
        {
        }
    }
}