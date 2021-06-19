using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace StatusFX
{
	public abstract class StatusEffect : ScriptableObject, IStatusEffect
	{
		public StatusEffectType EffectType => _effectType;
		public bool IsDebuff => _isDebuff;
		public abstract bool IsStarted { get; }

		[SerializeField] private bool _isDebuff;
		[SerializeField] private StatusEffectType _effectType;

		public abstract event IStatusEffect.StateChangeDelegate OnStarted;
		public abstract event IStatusEffect.StateChangeDelegate OnStopped;
		public abstract void Start();
		public abstract void Stop();
		public abstract void LinkNewTarget(IStatusFXCarrier newTarget);
		public abstract void UnlinkCurrentTarget();
		
		public IStatusEffect CreateInstance()
		{
			return Instantiate(this);
		}
	}
	
	public abstract class StatusEffect<TTarget> : StatusEffect where TTarget : IStatusFXCarrier
	{
		public override bool IsStarted => _isStarted;

		public override event IStatusEffect.StateChangeDelegate OnStarted;
		public override event IStatusEffect.StateChangeDelegate OnStopped;
		
		protected TTarget Target;
		private IDisposable _updateHandle;
		private bool _isStarted;

		protected virtual void Update()
		{
			OnUpdate();
		}

		protected virtual void OnUpdate() { }

		public override void Start()
		{
			_isStarted = true;
			OnStart();
		}

		protected virtual void OnStart() { }

		public override void Stop()
		{
			_isStarted = false;
			OnStop();
		}
		
		protected virtual void OnStop() { }

		public override void LinkNewTarget(IStatusFXCarrier newTarget)
		{
			if (!(newTarget is TTarget carrier))
				throw new ArgumentException(nameof(newTarget));

			LinkNewTarget(carrier);
		}

		public void LinkNewTarget([NotNull] TTarget newTarget)
		{
			if (newTarget == null)
				throw new ArgumentNullException(nameof(newTarget));

			if (newTarget.StatusFX.HasStatusEffectImplemented(EffectType))
				throw new InvalidOperationException($"New target already has implemented {EffectType}");

			if (Target?.StatusFX.HasStatusEffectImplemented(EffectType) ?? false)
				UnlinkCurrentTarget();

			Target = newTarget;
			_updateHandle = Target.GetUpdateObservable().Subscribe(_ => Update());
		}

		public override void UnlinkCurrentTarget()
		{
			if (Target != null)
			{
				Stop();
				_updateHandle?.Dispose();
				Target = default!;
			}
		}
	}
}