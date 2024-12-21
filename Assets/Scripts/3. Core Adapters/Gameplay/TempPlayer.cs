using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using PEntities.Gameplay.Combat;
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
        
        private List<IShipMorph> _shipMorphs;
        private int _currentMorph;

        [Inject]
        public void Construct(IMortal sharedHealth, IShipMorph[] shipMorphs)
        {
            _disposables = new CompositeDisposable();
            sharedHealth.IsDead
                .Subscribe(isDead => gameObject.SetActive(!isDead))
                .AddTo(_disposables);
            _shipMorphs = shipMorphs.ToList();
        }

        public void Update()
        {
            Shoot();
            ChangeMorph();
        }

        private void FixedUpdate()
        {
            Move();
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
        private IShipMorph CurrentMorph => _shipMorphs[_currentMorph];

        private void ChangeMorph()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _currentMorph++;
                if (_currentMorph >= _shipMorphs.Count)
                    _currentMorph = 0;
            }
        }

        [Button("Take 10 Damage")]
        private void TakeDamage()
        {
            CurrentMorph.ApplyDamage(10);
            print(CurrentMorph.CurrentHealth + "/" + CurrentMorph.MaximumHealth);
        }

        [Button("Heal 10 Points")]
        private void Heal()
        {
            CurrentMorph.ApplyDamage(-10);
            print(CurrentMorph.CurrentHealth + "/" + CurrentMorph.MaximumHealth);
        }

        private void Shoot()
        {
            var currentWeapon = CurrentMorph.CurrentWeapon;
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
            CurrentMorph.Move(moveDirection);
        }

        private Vector2 DefineDirection()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

    }
}