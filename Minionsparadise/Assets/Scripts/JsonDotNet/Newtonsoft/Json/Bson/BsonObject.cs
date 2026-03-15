namespace Newtonsoft.Json.Bson
{
	internal class BsonObject : global::Newtonsoft.Json.Bson.BsonToken, global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Bson.BsonProperty>, global::System.Collections.IEnumerable
	{
		private readonly global::System.Collections.Generic.List<global::Newtonsoft.Json.Bson.BsonProperty> _children = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Bson.BsonProperty>();

		public override global::Newtonsoft.Json.Bson.BsonType Type
		{
			get
			{
				return global::Newtonsoft.Json.Bson.BsonType.Object;
			}
		}

		public void Add(string name, global::Newtonsoft.Json.Bson.BsonToken token)
		{
			_children.Add(new global::Newtonsoft.Json.Bson.BsonProperty
			{
				Name = new global::Newtonsoft.Json.Bson.BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		public global::System.Collections.Generic.IEnumerator<global::Newtonsoft.Json.Bson.BsonProperty> GetEnumerator()
		{
			return _children.GetEnumerator();
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
