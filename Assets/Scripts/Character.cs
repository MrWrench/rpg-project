using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] public Stats Stats = new Stats();

	private void Start()
	{
		Stats.Reset();
	}
}