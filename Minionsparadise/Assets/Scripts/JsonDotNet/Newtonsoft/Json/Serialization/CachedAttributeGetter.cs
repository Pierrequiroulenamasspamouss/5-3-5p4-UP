namespace Newtonsoft.Json.Serialization
{
	internal static class CachedAttributeGetter<T> where T : global::System.Attribute
	{
		private static readonly global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Reflection.ICustomAttributeProvider, T> TypeAttributeCache = new global::Newtonsoft.Json.Utilities.ThreadSafeStore<global::System.Reflection.ICustomAttributeProvider, T>(global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetAttribute<T>);

		public static T GetAttribute(global::System.Reflection.ICustomAttributeProvider type)
		{
			return TypeAttributeCache.Get(type);
		}
	}
}
