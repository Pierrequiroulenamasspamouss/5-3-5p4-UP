namespace Newtonsoft.Json.Serialization
{
	public interface IContractResolver
	{
		global::Newtonsoft.Json.Serialization.JsonContract ResolveContract(global::System.Type type);
	}
}
