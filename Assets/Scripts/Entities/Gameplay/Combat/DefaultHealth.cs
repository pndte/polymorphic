using System;
using R3;
using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public class DefaultHealth : IMortal, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable;

        public DefaultHealth(float current, float maximum) // TODO: move to config.
        {
            CurrentHealth = new ReactiveProperty<float>(current);
            MaximumHealth = new ReactiveProperty<float>(maximum);
            IsDead = new ReactiveProperty<bool>(false);
            _compositeDisposable = new CompositeDisposable();

            SubscribeAll();
        }

        public ReactiveProperty<float> CurrentHealth { get; }
        public ReactiveProperty<float> MaximumHealth { get; }
        public ReactiveProperty<bool> IsDead { get; }
        
        public void ApplyDamage(float value)
        {
            CurrentHealth.Value -= value;
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        private void SubscribeAll()
        {
            EnableDeath();
            EnableCurrentHealthValidation();
        }

        private void EnableDeath()
        {
            CurrentHealth
                .Where(currentHealth => currentHealth <= 0)
                .Subscribe(_ => IsDead.Value = true)
                .AddTo(_compositeDisposable);

            CurrentHealth
                .Where(currentHealth => currentHealth > 0)
                .Subscribe(_ => IsDead.Value = false)
                .AddTo(_compositeDisposable);
        }

        private void EnableCurrentHealthValidation()
        {
            CurrentHealth.Subscribe(newValue =>
                    CurrentHealth.Value = Mathf.Clamp(newValue, 0, MaximumHealth.Value))
                .AddTo(_compositeDisposable);
        }
    }
}