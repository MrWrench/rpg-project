using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wrenge.StatusFX
{
	public class StatusEffect
	{
		public int MaxStacks { get; set; }
		public int CurrentStacks { get; private set; }
		public ITimeProvider Time { get; protected set; }

		private readonly Dictionary<Type, StatusComponent> _componentsMap = new Dictionary<Type, StatusComponent>();
		private readonly List<StatusComponent> _componentsList = new List<StatusComponent>();

		public TComponent GetComponent<TComponent>() where TComponent : StatusComponent
		{
			if (_componentsMap.TryGetValue(typeof(TComponent), out var component))
				return component as TComponent;
			return null;
		}

		public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : StatusComponent
		{
			component = GetComponent<TComponent>();
			return component != null;
		}

		public bool HasComponent<TComponent>() => _componentsMap.ContainsKey(typeof(TComponent));

		public void AddComponent<TComponent>(TComponent component) where TComponent : StatusComponent
		{
			if (HasComponent<TComponent>())
				throw new ArgumentException($"{nameof(TComponent)} already exists");

			_componentsMap.Add(typeof(TComponent), component);
			_componentsList.Add(component);
			component.NotifyAdded(this);
		}

		public void RemoveComponent<TComponent>(TComponent component) where TComponent : StatusComponent
		{
			if (!HasComponent<TComponent>())
				return;

			_componentsMap.Remove(typeof(TComponent));
			_componentsList.Remove(component);
			component.NotifyRemoved();
		}

		public void ChangeStacks(int deltaStacks)
		{
			var oldStacks = CurrentStacks;
			CurrentStacks = Mathf.Clamp(CurrentStacks + deltaStacks, 0, MaxStacks);
			var actualDelta = CurrentStacks - oldStacks;
			
			foreach (var component in _componentsList) 
				component.NotifyStacksChanged(actualDelta);
		}

		public void Tick()
		{
			foreach (var component in _componentsList)
			{
				if(component.IsActive)
					component.Tick();
			}
		}
	}
}