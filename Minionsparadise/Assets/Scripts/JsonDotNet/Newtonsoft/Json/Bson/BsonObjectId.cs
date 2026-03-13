namespace Newtonsoft.Json.Bson
{
	public class BsonObjectId
	{
		public byte[] Value { get; private set; }

		public BsonObjectId(byte[] value)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new global::System.Exception("An ObjectId must be 12 bytes");
			}
			Value = value;
		}
	}
}
