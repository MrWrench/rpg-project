namespace Wrenge.StatusFX
{
	public abstract class StatusComponent
	{
		public bool IsActive { get; set; }
		protected StatusEffect Owner { get; private set; }
		protected ITimeProvider Time => Owner.Time;
		
		public void NotifyAdded(StatusEffect owner)
		{
			Owner = owner;
			OnAdded();
		}
		
		protected virtual void OnAdded() { }
		
		public void NotifyRemoved()
		{
			var previousOwner = Owner;
			Owner = null;
			OnRemoved(previousOwner);
		}
		
		protected virtual void OnRemoved(StatusEffect previousOwner) { }

		public void NotifyStacksChanged(int deltaStacks)
		{
			OnStacksChanged(deltaStacks);
		}
		
		protected virtual void OnStacksChanged(int deltaStacks) { }
		
		public virtual void Tick() { }
	}
}