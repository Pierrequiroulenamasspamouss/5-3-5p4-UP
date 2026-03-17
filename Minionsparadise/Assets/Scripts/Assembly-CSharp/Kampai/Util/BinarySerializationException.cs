namespace Kampai.Util
{
	public class BinarySerializationException : global::System.Exception
	{
		public BinarySerializationException()
		{
		}

		public BinarySerializationException(string message)
			: base(message)
		{
		}

		public BinarySerializationException(string message, global::System.Exception inner)
			: base(message, inner)
		{
		}
	}
}
