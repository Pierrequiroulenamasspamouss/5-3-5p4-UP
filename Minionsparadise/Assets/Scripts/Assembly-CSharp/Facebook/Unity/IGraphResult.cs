namespace Facebook.Unity
{
	public interface IGraphResult : global::Facebook.Unity.IResult
	{
		global::System.Collections.Generic.IList<object> ResultList { get; }

		global::UnityEngine.Texture2D Texture { get; }
	}
}
