using EditorAttributes;
using PUseCases.Gameplay;
using R3;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PCoreAdapters.Gameplay
{
    public class TempPlayer : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Vector2> _shooted;
        private CompositeDisposable _disposables;
        
        private IShipMorph _shipMorph;

        [Inject]
        public void Construct(IShipMorph shipMorph)
        {
            _shipMorph = shipMorph;
            _disposables = new CompositeDisposable();
            _shipMorph.IsDead
                .Subscribe(isDead => gameObject.SetActive(!isDead))
                .AddTo(_disposables);
        }

        public void Update()
        {
            Shoot();
        }

        private void FixedUpdate()
        {
            Move();
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        [Button("Take 10 Damage")]
        private void TakeDamage()
        {
            _shipMorph.ApplyDamage(10);
            print(_shipMorph.CurrentHealth + "/" + _shipMorph.MaximumHealth);
        }

        [Button("Heal 10 Points")]
        private void Heal()
        {
            _shipMorph.ApplyDamage(-10);
            print(_shipMorph.CurrentHealth + "/" + _shipMorph.MaximumHealth);
        }

        private void Shoot()
        {
            var currentWeapon = _shipMorph.CurrentWeapon;
            if (Input.GetMouseButton(0) && currentWeapon.Reloaded)
            {
                currentWeapon.Shoot(transform.up);
                currentWeapon.ReloadAsync();
                
                _shooted.Invoke(transform.up);
            }
        }

        private void Move()
        {
            var moveDirection = DefineDirection();
            _shipMorph.Move(moveDirection);
        }

        private Vector2 DefineDirection()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

    }
}