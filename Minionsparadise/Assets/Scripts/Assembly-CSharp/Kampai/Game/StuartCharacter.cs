namespace Kampai.Game
{
	public class StuartCharacter : global::Kampai.Game.FrolicCharacter<global::Kampai.Game.StuartCharacterDefinition>
	{
		public bool WasHonorGuest { get; set; }

		public StuartCharacter(global::Kampai.Game.StuartCharacterDefinition def)
			: base(def)
		{
			Name = "Stuart";
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "WASHONORGUEST":
				reader.Read();
				WasHonorGuest = global::System.Convert.ToBoolean(reader.Value);
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
			writer.WritePropertyName("WasHonorGuest");
			writer.WriteValue(WasHonorGuest);
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			return go.AddComponent<global::Kampai.Game.View.StuartView>();
		}
	}
}
