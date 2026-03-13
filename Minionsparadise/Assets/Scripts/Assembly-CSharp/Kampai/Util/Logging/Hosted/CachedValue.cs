namespace Kampai.Util.Logging.Hosted
{
	internal sealed class CachedValue<T> where T : class
	{
		private T value;

		private bool fresh;

		private bool retryIfNull = true;

		private global::System.Func<T> valueSetter;

		public CachedValue(global::System.Func<T> valueSetter)
		{
			this.valueSetter = valueSetter;
		}

		public void SetFresh(bool fresh)
		{
			this.fresh = fresh;
		}

		public T GetValue()
		{
			if ((retryIfNull && value == null) || !fresh)
			{
				value = valueSetter();
				fresh = true;
			}
			return value;
		}
	}
}
