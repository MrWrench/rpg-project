using System;
using UniRx;

public interface IUpdateProvider
{
	IObservable<Unit> GetUpdateObservable();
}