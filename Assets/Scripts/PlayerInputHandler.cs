using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerInputHandler : MonoBehaviour
{
	private CharacterMotor _motor;
	private void Awake()
	{
		_motor = GetComponent<CharacterMotor>();
	}

	public void OnMove(InputValue value)
	{
		_motor.MoveInput = value.Get<Vector2>();
	}

	public void OnLook(InputValue value)
	{
		var vectorValue = value.Get<Vector2>();
		Camera.main.ViewportPointToRay(Input.mousePosition + Vector3.forward * 100);
		// _motor.LookDir = (transform.position - Camera.main.ScreenToWorldPoint(new Vector3(vectorValue.x, vectorValue.y, 0))).normalized;
	}
}