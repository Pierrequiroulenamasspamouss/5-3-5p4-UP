namespace Kampai.Game
{
	public class PlayerDefinitionConverter : global::Newtonsoft.Json.JsonConverter
	{
		private global::Kampai.Game.DefinitionService defService;

		public PlayerDefinitionConverter(global::Kampai.Game.DefinitionService defService)
		{
			this.defService = defService;
		}

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			defService.SetInitialPlayer(jObject.ToString());
			return null;
		}

		public override bool CanConvert(global::System.Type objectType)
		{
			if (objectType.BaseType == null)
			{
				return false;
			}
			if (typeof(global::Kampai.Game.PlayerVersion).IsAssignableFrom(objectType))
			{
				return true;
			}
			return false;
		}
	}
}
