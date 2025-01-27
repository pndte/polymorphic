using PEntities.Gameplay.Behaviour;
using PUseCases.Gameplay;
using R3;
using UnityEngine;
using Zenject;

namespace PCoreAdapters.Gameplay
{
    public class TempEnemy : MonoBehaviour
    {
        private CompositeDisposable _disposables;
        private IShipMorph _shipMorph;
        private RootNode _rootNode;

        [Inject]
        public void Construct(IShipMorph shipMorph, RootNode startNode)
        {
            _shipMorph = shipMorph;
            _disposables = new CompositeDisposable();

            _shipMorph.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ =>
                {
                    transform.position = new Vector3(99999, 99999, 99999);
                })
                .AddTo(_disposables);
            _shipMorph.IsDead
                .Subscribe(isDead => gameObject.SetActive(!isDead))
                .AddTo(_disposables);
            
            _rootNode = startNode;
            
        }

        private void FixedUpdate()
        {
            _rootNode.Evaluate();
        }
        
        public IShipMorph Morph => _shipMorph;
        public bool IsSeparating { get; set; }
        public ReactiveProperty<bool> isDead { get; } = new ReactiveProperty<bool>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            var bullet = other.GetComponent<MonoBullet>();
            _shipMorph.ApplyDamage(bullet.Data.Damage.Value);
            
            if (!bullet.IsReset.Value)
               bullet.Reset.Execute(bullet);
        }
    }
}