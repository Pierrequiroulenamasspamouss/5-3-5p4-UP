namespace Newtonsoft.Json.Bson
{
	internal class BsonString : global::Newtonsoft.Json.Bson.BsonValue
	{
		public int ByteCount { get; set; }

		public bool IncludeLength { get; set; }

		public BsonString(object value, bool includeLength)
			: base(value, global::Newtonsoft.Json.Bson.BsonType.String)
		{
			IncludeLength = includeLength;
		}
	}
}
