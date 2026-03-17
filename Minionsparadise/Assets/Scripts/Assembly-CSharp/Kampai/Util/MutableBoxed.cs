namespace Kampai.Util
{
	public class MutableBoxed<T> : global::Kampai.Util.Boxed<T>
	{
		public MutableBoxed(T value)
			: base(value)
		{
		}

		public void Set(T value)
		{
			base.value = value;
		}
	}
}
