using System;
using JetBrains.Annotations;
using UniRx;

namespace StatusFX
{
	public abstract class StatusEffect<TTarget, TConfig> : IStatusEffect where TTarget : IStatusFXCarrier where TConfig : IStatusEffectConfig
	{
		public abstract StatusEffectType EffectType { get; }
		public bool IsStarted { get; private set; }
		public abstract bool IsDebuff { get; }
		public event IStatusEffect.StartDelegate OnStarted;
		public event IStatusEffect.StopDelegate OnStopped;

		protected TTarget Target;
		protected TConfig Config;
		private IDisposable _updateHandle;

		protected virtual void Update()
		{
			OnUpdate();
		}

		protected virtual void OnUpdate() { }

		public virtual void Start()
		{
			IsStarted = true;
			OnStart();
			OnStarted?.Invoke(this);
		}

		protected virtual void OnStart() { }

		public virtual void Stop()
		{
			IsStarted = false;
			OnStop();
			OnStopped?.Invoke(this);
		}
		
		protected virtual void OnStop() { }

		public void LinkNewTarget(IStatusFXCarrier newTarget)
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
			Target.StatusFX.ImplementStatusEffect(this);
			_updateHandle = Target.GetUpdateObservable().Subscribe(_ => Update());
		}

		public void UnlinkCurrentTarget()
		{
			if (Target != null)
			{
				Stop();
				_updateHandle?.Dispose();
				Target.StatusFX.UnimplementStatusEffect(this);
				Target = default!;
			}
		}

		void IStatusEffect.SetConfig(IStatusEffectConfig config)
		{
			Config = (TConfig) config;
		}
		
	}
}