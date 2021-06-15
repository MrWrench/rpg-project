using System;
using JetBrains.Annotations;
using UniRx;

namespace StatusFX.Generic
{
	public abstract class StatusEffect<T> : IStatusEffect<T> where T : IStatusFXCarrier?
	{
		public abstract EnumStatusType type { get; }
		public bool isStarted { get; private set; }
		public abstract bool isDebuff { get; }
		public event IStatusEffect.StartDelegate? onStarted;
		public event IStatusEffect.StopDelegate? onStopped;

		protected T target = default!;
		private IDisposable? updateHandle;

		protected virtual void Update()
		{
			OnUpdate();
		}

		protected virtual void OnUpdate() { }

		public virtual void Start()
		{
			isStarted = true;
			OnStart();
			onStarted?.Invoke(this);
		}

		protected virtual void OnStart() { }

		public virtual void Stop()
		{
			isStarted = false;
			OnStop();
			onStopped?.Invoke(this);
		}
		
		protected virtual void OnStop() { }

		public void LinkNewTarget(IStatusFXCarrier newTarget)
		{
			if (!(newTarget is T carrier))
				throw new ArgumentException(nameof(newTarget));

			LinkNewTarget(carrier);
		}

		public void LinkNewTarget([NotNull] T newTarget)
		{
			if (newTarget == null)
				throw new ArgumentNullException(nameof(newTarget));

			if (newTarget.statusFX.HasStatusEffectImplemented(type))
				throw new InvalidOperationException($"New target already has implemented {type}");

			if (target?.statusFX.HasStatusEffectImplemented(type) ?? false)
				UnlinkCurrentTarget();

			target = newTarget;
			target.statusFX.ImplementStatusEffect(this);
			updateHandle = target.GetUpdateObservable().Subscribe(_ => Update());
		}

		public void UnlinkCurrentTarget()
		{
			if (target != null)
			{
				Stop();
				updateHandle?.Dispose();
				target.statusFX.UnimplementStatusEffect(this);
				target = default!;
			}
		}
	}
}