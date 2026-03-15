namespace Kampai.Util
{
	public class Boxed<T> : global::Kampai.Util.IBoxed
	{
		protected T value;

		public T Value
		{
			get
			{
				return value;
			}
		}

		public Boxed(T value)
		{
			this.value = value;
		}
	}
}
