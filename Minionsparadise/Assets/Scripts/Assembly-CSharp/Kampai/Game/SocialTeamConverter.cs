namespace Kampai.Game
{
	public class SocialTeamConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.SocialTeam>
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.TimedSocialEventDefinition def;

		public SocialTeamConverter(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.Null)
			{
				global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
				global::Newtonsoft.Json.Linq.JProperty jProperty = jObject.Property("socialEventId");
				int id = global::Newtonsoft.Json.Linq.LinqExtensions.Value<int>(jProperty.Value);
				def = definitionService.Get<global::Kampai.Game.TimedSocialEventDefinition>(id);
				reader = jObject.CreateReader();
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.SocialTeam Create(global::System.Type objectType)
		{
			return new global::Kampai.Game.SocialTeam(def);
		}
	}
}
