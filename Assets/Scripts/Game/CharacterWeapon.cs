using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider))]
    public class CharacterWeapon : MonoBehaviour
    {
        public IDamageable Owner { get; set; }
        public DamageInfo DamageInfo;
        private Collider _col;
        private readonly HashSet<IDamageable> _hitObjects = new();

        private void Awake()
        {
            _col = GetComponent<Collider>();
        }

        public void SetActive(bool state)
        {
            _col.enabled = state;
            _hitObjects.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();
            if(damageable == null || damageable == Owner || _hitObjects.Contains(damageable))
                return;
            HitObject(damageable);
        }

        private void HitObject(IDamageable target)
        {
            target.ApplyDamage(DamageInfo);
            _hitObjects.Add(target);
        }
    }
}
