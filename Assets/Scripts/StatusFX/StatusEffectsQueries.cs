using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace StatusFX
{
	internal static class StatusEffectsQueries
	{
		internal static IReadOnlyCollection<T> FindFriendsInSphere<T>([NotNull] T unit,
			Vector3 position, float radius) where T : ICombatUnit
		{
			if (unit == null)
				throw new ArgumentNullException(nameof(unit));
			if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius));

			var colliders = Physics.OverlapSphere(position, radius);
			if (colliders.Length <= 0)
				return Array.Empty<T>();

			return colliders.Select(x => x.GetComponent<T>())
				.Where(x => x != null && x.team == unit.team && !Equals(x, unit))
				.ToArray();
		}

		internal static IReadOnlyCollection<T> FindOtherInSphere<T>([NotNull] T unit,
			Vector3 position, float radius) where T : ICombatUnit
		{
			if (unit == null)
				throw new ArgumentNullException(nameof(unit));
			if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius));

			var colliders = Physics.OverlapSphere(position, radius);
			if (colliders.Length <= 0)
				return Array.Empty<T>();

			return colliders.Select(x => x.GetComponent<T>())
				.Where(x => x != null && x.team != unit.team)
				.ToArray();
		}
	}
}