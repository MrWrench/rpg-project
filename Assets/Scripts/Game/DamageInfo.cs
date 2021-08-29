namespace Game
{
    public struct DamageInfo
    {
        public DamageType Type;
        public float HealthAmount;
        public float PoiseAmount;
        public float Multiplier; // Used for time related scaling
        public object Inflictor;
        public Character Attacker;

        public static DamageInfo Default() => new DamageInfo {Multiplier = 1, Type = DamageType.Physical};

        public DamageInfo SetType(DamageType type)
        {
            Type = type;
            return this;
        }

        public DamageInfo SetHealth(float value)
        {
            HealthAmount = value;
            return this;
        }

        public DamageInfo SetPoise(float value)
        {
            PoiseAmount = value;
            return this;
        }

        public DamageInfo SetMultiplier(float value)
        {
            Multiplier = value;
            return this;
        }

        public DamageInfo SetInflictor(object value)
        {
            Inflictor = value;
            return this;
        }

        public DamageInfo SetAttacker(Character value)
        {
            Attacker = value;
            return this;
        }
    }
}
