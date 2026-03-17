namespace Facebook.Unity
{
	internal class GraphResult : global::Facebook.Unity.ResultBase, global::Facebook.Unity.IGraphResult, global::Facebook.Unity.IResult
	{
		public global::System.Collections.Generic.IList<object> ResultList { get; private set; }

		public global::UnityEngine.Texture2D Texture { get; private set; }

		internal GraphResult(global::UnityEngine.WWW result)
			: base(new global::Facebook.Unity.ResultContainer(result.text), result.error, false)
		{
			Init(RawResult);
			if (result.error == null)
			{
				Texture = result.texture;
			}
		}

		private void Init(string rawResult)
		{
			if (string.IsNullOrEmpty(rawResult))
			{
				return;
			}
			object obj = global::Facebook.MiniJSON.Json.Deserialize(RawResult);
			global::System.Collections.Generic.IDictionary<string, object> dictionary = obj as global::System.Collections.Generic.IDictionary<string, object>;
			if (dictionary != null)
			{
				ResultDictionary = dictionary;
				return;
			}
			global::System.Collections.Generic.IList<object> list = obj as global::System.Collections.Generic.IList<object>;
			if (list != null)
			{
				ResultList = list;
			}
		}
	}
}
