using System;
using UnityEngine;

public class EnterStateToParam : StateMachineBehaviour
{
	[SerializeField] private string _parameterName = "";
	[SerializeField] private bool _value = true;
	private int _parameterHash;

	public void Awake()
	{
		_parameterHash = Animator.StringToHash(_parameterName);
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool(_parameterHash, _value);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool(_parameterHash, !_value);
	}
}