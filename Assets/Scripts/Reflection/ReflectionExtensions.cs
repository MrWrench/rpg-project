using System;
using System.Reflection;

namespace Reflection
{
	public static class ReflectionExtensions
	{
		public static T GetCustomAttribute<T>(this Type type) where T : Attribute
		{
			return (T) type.GetCustomAttribute(typeof(T));
		}
	}
}