namespace Newtonsoft.Json.Bson
{
	internal class BsonRegex : global::Newtonsoft.Json.Bson.BsonToken
	{
		public global::Newtonsoft.Json.Bson.BsonString Pattern { get; set; }

		public global::Newtonsoft.Json.Bson.BsonString Options { get; set; }

		public override global::Newtonsoft.Json.Bson.BsonType Type
		{
			get
			{
				return global::Newtonsoft.Json.Bson.BsonType.Regex;
			}
		}

		public BsonRegex(string pattern, string options)
		{
			Pattern = new global::Newtonsoft.Json.Bson.BsonString(pattern, false);
			Options = new global::Newtonsoft.Json.Bson.BsonString(options, false);
		}
	}
}
