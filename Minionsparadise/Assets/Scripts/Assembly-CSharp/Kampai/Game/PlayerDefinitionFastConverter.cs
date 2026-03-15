namespace Kampai.Game
{
	public class PlayerDefinitionFastConverter : global::Kampai.Util.FastJsonConverter<global::Kampai.Game.PlayerVersion>
	{
		private global::Kampai.Game.DefinitionService defService;

		public PlayerDefinitionFastConverter(global::Kampai.Game.DefinitionService defService)
		{
			this.defService = defService;
		}

		public global::Kampai.Game.PlayerVersion ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			defService.SetInitialPlayer(jObject.ToString());
			return null;
		}

		public global::Kampai.Game.PlayerVersion Create()
		{
			return null;
		}
	}
}
