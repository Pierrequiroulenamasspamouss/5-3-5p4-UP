namespace Newtonsoft.Json.Bson
{
	internal class BsonValue : global::Newtonsoft.Json.Bson.BsonToken
	{
		private object _value;

		private global::Newtonsoft.Json.Bson.BsonType _type;

		public object Value
		{
			get
			{
				return _value;
			}
		}

		public override global::Newtonsoft.Json.Bson.BsonType Type
		{
			get
			{
				return _type;
			}
		}

		public BsonValue(object value, global::Newtonsoft.Json.Bson.BsonType type)
		{
			_value = value;
			_type = type;
		}
	}
}
