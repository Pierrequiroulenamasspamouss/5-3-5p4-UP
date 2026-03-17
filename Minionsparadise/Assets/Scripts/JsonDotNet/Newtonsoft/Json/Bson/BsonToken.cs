namespace Newtonsoft.Json.Bson
{
	internal abstract class BsonToken
	{
		public abstract global::Newtonsoft.Json.Bson.BsonType Type { get; }

		public global::Newtonsoft.Json.Bson.BsonToken Parent { get; set; }

		public int CalculatedSize { get; set; }
	}
}
