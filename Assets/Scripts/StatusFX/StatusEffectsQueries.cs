using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace StatusFX
{
	internal static class StatusEffectsQueries
	{
		internal static IReadOnlyCollection<Character> FindFriendsInSphere([NotNull] Character character,
			Vector3 position, float radius)
		{
			if (character == null)
				throw new ArgumentNullException(nameof(character));
			if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius));

			var colliders = Physics.OverlapSphere(position, radius);
			if (colliders.Length <= 0)
				return Array.Empty<Character>();

			return colliders.Select(x => x.GetComponent<Character>())
				.Where(x => x != null && x.team == character.team && x != character).ToArray();
		}

		internal static IReadOnlyCollection<Character> FindOtherInSphere([NotNull] Character character,
			Vector3 position, float radius)
		{
			if (character == null)
				throw new ArgumentNullException(nameof(character));
			if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius));

			var colliders = Physics.OverlapSphere(position, radius);
			if (colliders.Length <= 0)
				return Array.Empty<Character>();

			return colliders.Select(x => x.GetComponent<Character>())
				.Where(x => x != null && x.team != character.team).ToArray();
		}
	}
}