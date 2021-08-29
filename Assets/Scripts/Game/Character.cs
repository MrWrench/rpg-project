using System;
using StatusFX;
using StatusFX.Elemental;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, IDamageable
    {
        [SerializeField] public Stats Stats = new Stats();
        public UnitTeam Team { get; set; }
        public readonly StatusEffects StatusEffects = new StatusEffects();
        public event Action<DamageInfo> OnDamageTaken;

        protected virtual void Start()
        {
            Stats.Reset();
            SetupStatuses();
        }

        private void SetupStatuses()
        {
            StatusEffects.Add(new FireStatus(this));
            StatusEffects.Add(new CryoStatus(this));
            StatusEffects.Add(new HydroStatus(this));
            StatusEffects.Add(new ElectroStatus(this));
            StatusEffects.Add(new PoisonStatus(this));
        }

        public void ApplyDamage(DamageInfo info)
        {
            var healthDamage = info.HealthAmount * info.Multiplier;
            Stats.Health -= healthDamage;
            Stats.Poise -= (info.PoiseAmount + Stats.PoiseDamageDebuff) * info.Multiplier;
            OnDamageTaken?.Invoke(info);

            Debug.Log($"{name} took damage: -{healthDamage}");
            Debug.Log($"Health of {name}: {Stats.Health}/{Stats.MaxHealth}");
            Debug.Log($"Poise of {name}: {Stats.Poise}/{Stats.MaxPoise}");
        }

        private void Update()
        {
            StatusEffects.Tick();
        }
    }
}
