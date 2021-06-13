using StatusFX;

public interface ICombatUnit : IDamageable, IStatusFXCarrier, ISceneObject, IStatsCarrier
{
	EnumTeam team { get; }
}