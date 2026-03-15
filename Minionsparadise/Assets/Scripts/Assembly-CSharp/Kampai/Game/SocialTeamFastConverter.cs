namespace Kampai.Game
{
	public class SocialTeamFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.SocialTeam>
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.TimedSocialEventDefinition def;

		public SocialTeamFastConverter(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public override global::Kampai.Game.SocialTeam ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.Null)
			{
				global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
				global::Newtonsoft.Json.Linq.JProperty jProperty = jObject.Property("socialEventId");
				int id = global::Newtonsoft.Json.Linq.LinqExtensions.Value<int>(jProperty.Value);
				def = definitionService.Get<global::Kampai.Game.TimedSocialEventDefinition>(id);
				reader = jObject.CreateReader();
			}
			return base.ReadJson(reader, converters);
		}

		public override global::Kampai.Game.SocialTeam Create()
		{
			return new global::Kampai.Game.SocialTeam(def);
		}
	}
}
