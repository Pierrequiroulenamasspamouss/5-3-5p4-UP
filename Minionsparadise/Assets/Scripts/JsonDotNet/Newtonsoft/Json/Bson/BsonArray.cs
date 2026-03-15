namespace Newtonsoft.Json.Bson
{
	internal class BsonArray : global::Newtonsoft.Json.Bson.BsonToken, global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Bson.BsonToken>, global::System.Collections.IEnumerable
	{
		private readonly global::System.Collections.Generic.List<global::Newtonsoft.Json.Bson.BsonToken> _children = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Bson.BsonToken>();

		public override global::Newtonsoft.Json.Bson.BsonType Type
		{
			get
			{
				return global::Newtonsoft.Json.Bson.BsonType.Array;
			}
		}

		public void Add(global::Newtonsoft.Json.Bson.BsonToken token)
		{
			_children.Add(token);
			token.Parent = this;
		}

		public global::System.Collections.Generic.IEnumerator<global::Newtonsoft.Json.Bson.BsonToken> GetEnumerator()
		{
			return _children.GetEnumerator();
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
