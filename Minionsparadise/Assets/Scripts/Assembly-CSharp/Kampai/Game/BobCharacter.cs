namespace Kampai.Game
{
	public class BobCharacter : global::Kampai.Game.FrolicCharacter<global::Kampai.Game.BobCharacterDefinition>
	{
		public bool HasShownExpansionNarrative { get; set; }

		public BobCharacter(global::Kampai.Game.BobCharacterDefinition def)
			: base(def)
		{
			Name = "Bob";
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "HASSHOWNEXPANSIONNARRATIVE":
				reader.Read();
				HasShownExpansionNarrative = global::System.Convert.ToBoolean(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			writer.WritePropertyName("HasShownExpansionNarrative");
			writer.WriteValue(HasShownExpansionNarrative);
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			return go.AddComponent<global::Kampai.Game.View.BobView>();
		}
	}
}
