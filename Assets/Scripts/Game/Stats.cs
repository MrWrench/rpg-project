using System;

namespace Game
{
    [Serializable]
    public class Stats
    {
        public float MaxHealth = 100;
        public float MaxPoise = 100;

        [NonSerialized] public float Health;
        [NonSerialized] public float Poise;
        [NonSerialized] public float StatusDuration;
        [NonSerialized] public float PoiseDamageDebuff;

        public void Reset()
        {
            Health = MaxHealth;
            Poise = MaxPoise;
            StatusDuration = 1;
            PoiseDamageDebuff = 0;
        }
    }
}
