namespace Kampai.Game
{
	public class NamedCharacterDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.NamedCharacterDefinition>
	{
		private global::Kampai.Game.NamedCharacterType type;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("type") != null)
			{
				string value = jObject.Property("type").Value.ToString();
				type = (global::Kampai.Game.NamedCharacterType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.NamedCharacterType), value);
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.NamedCharacterDefinition Create(global::System.Type objectType)
		{
			switch (type)
			{
			case global::Kampai.Game.NamedCharacterType.BOB:
				return new global::Kampai.Game.BobCharacterDefinition();
			case global::Kampai.Game.NamedCharacterType.VILLAIN:
				return new global::Kampai.Game.VillainDefinition();
			case global::Kampai.Game.NamedCharacterType.PHIL:
				return new global::Kampai.Game.PhilCharacterDefinition();
			case global::Kampai.Game.NamedCharacterType.STUART:
				return new global::Kampai.Game.StuartCharacterDefinition();
			case global::Kampai.Game.NamedCharacterType.KEVIN:
				return new global::Kampai.Game.KevinCharacterDefinition();
			case global::Kampai.Game.NamedCharacterType.TSM:
				return new global::Kampai.Game.TSMCharacterDefinition();
			case global::Kampai.Game.NamedCharacterType.SPECIAL_EVENT:
				return new global::Kampai.Game.SpecialEventCharacterDefinition();
			default:
				return null;
			}
		}
	}
}
