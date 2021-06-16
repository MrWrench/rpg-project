using System;

namespace GameDebug
{
	public static class CharacterDebug
	{
		public static event Action<Character>? OnSpawn;
		public static event Action? OnDestroyed;
		public static event Action<Character>? OnEnabled;
		public static event Action<Character>? OnDisabled;

		public static void InvokeSpawn(Character character) => OnSpawn?.Invoke(character);

		public static void InvokeDestroyed() => OnDestroyed?.Invoke();

		public static void InvokeEnabled(Character character) => OnEnabled?.Invoke(character);

		public static void InvokeDisabled(Character character) => OnDisabled?.Invoke(character);
	}
}