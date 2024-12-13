using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PUseCases.Gameplay
{
    public class TempPlayer : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Vector2> _shooted;
        
        private IShipMorph _shipMorph;

        [Inject]
        public void Construct(IShipMorph shipMorph)
        {
            _shipMorph = shipMorph;
        }

        public void Update()
        {
            Shoot();
        }

        private void FixedUpdate()
        {
            Move();
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