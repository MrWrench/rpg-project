using StatusFX;

public interface ICombatUnit : IDamageable, IStatusFXCarrier, ISceneObject, IStatsCarrier
{
	UnitTeam Team { get; }
}