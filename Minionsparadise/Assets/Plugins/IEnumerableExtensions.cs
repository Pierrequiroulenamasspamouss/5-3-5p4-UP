public static class IEnumerableExtensions
{
	public static global::System.Collections.Generic.HashSet<T> ToHashSet<T>(this global::System.Collections.Generic.IEnumerable<T> source)
	{
		return new global::System.Collections.Generic.HashSet<T>(source);
	}
}
