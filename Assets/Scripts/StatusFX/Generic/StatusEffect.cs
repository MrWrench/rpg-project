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
		public event IStatusEffect.StopDelegate? onStoped;

		protected T target;
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
			onStoped?.Invoke(this);
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

			if (newTarget.HasStatusEffectImplemented(type))
				throw new InvalidOperationException($"New target already has implemented {type}");

			if (target?.HasStatusEffectImplemented(type) ?? false)
				UnlinkCurrentTarget();

			target = newTarget;
			target.ImplementStatusEffect(this);
			updateHandle = target.GetUpdateObservable().Subscribe(_ => Update());
		}

		public void UnlinkCurrentTarget()
		{
			if (target != null)
			{
				Stop();
				updateHandle?.Dispose();
				target.UnimplementStatusEffect(this);
				target = default;
			}
		}
	}
}