namespace Discord.Unity
{
	public interface IGraphResult : global::Discord.Unity.IResult
	{
		global::System.Collections.Generic.IList<object> ResultList { get; }

		global::UnityEngine.Texture2D Texture { get; }
	}
}
