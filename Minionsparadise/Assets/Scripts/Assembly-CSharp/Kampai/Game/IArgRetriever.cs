namespace Kampai.Game
{
	public interface IArgRetriever
	{
		int Length { get; }

		int GetInt(int index);

		float GetFloat(int index);

		string GetString(int index);

		bool GetBoolean(int index);

		object Get(int index, global::System.Type type);
	}
}
