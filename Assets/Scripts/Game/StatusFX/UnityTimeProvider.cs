using UnityEngine;
using Wrenge.StatusFX;

namespace StatusFX
{
	public class UnityTimeProvider : ITimeProvider
	{
		public float CurrentTime => Time.time;
		public float DeltaTime => Time.deltaTime;
	}
}