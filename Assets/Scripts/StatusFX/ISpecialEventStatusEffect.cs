using System;
using UniRx;

namespace StatusFX
{
	public interface ISpecialEventStatusEffect<out T> : IStatusEffect
	{
		IObservable<T> OnSpecialEventAsObservable();
	}
}