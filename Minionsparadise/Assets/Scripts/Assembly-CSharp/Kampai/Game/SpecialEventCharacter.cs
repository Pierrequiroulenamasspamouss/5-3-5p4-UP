namespace Kampai.Game
{
	public class SpecialEventCharacter : global::Kampai.Game.FrolicCharacter<global::Kampai.Game.SpecialEventCharacterDefinition>
	{
		public bool HasShownIntroNarrative { get; set; }

		public int PreviousTaskUTCTime { get; set; }

		public int SpecialEventID { get; set; }

		public int PrestigeDefinitionID { get; set; }

		public SpecialEventCharacter(global::Kampai.Game.SpecialEventCharacterDefinition def)
			: base(def)
		{
			Name = "SpecialEventMinion";
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "HASSHOWNINTRONARRATIVE":
				reader.Read();
				HasShownIntroNarrative = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "PREVIOUSTASKUTCTIME":
				reader.Read();
				PreviousTaskUTCTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SPECIALEVENTID":
				reader.Read();
				SpecialEventID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PRESTIGEDEFINITIONID":
				reader.Read();
				PrestigeDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
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
			writer.WritePropertyName("HasShownIntroNarrative");
			writer.WriteValue(HasShownIntroNarrative);
			writer.WritePropertyName("PreviousTaskUTCTime");
			writer.WriteValue(PreviousTaskUTCTime);
			writer.WritePropertyName("SpecialEventID");
			writer.WriteValue(SpecialEventID);
			writer.WritePropertyName("PrestigeDefinitionID");
			writer.WriteValue(PrestigeDefinitionID);
		}

		public override global::Kampai.Game.View.NamedCharacterObject Setup(global::UnityEngine.GameObject go)
		{
			global::Kampai.Util.AI.Agent agent = go.GetComponent<global::Kampai.Util.AI.Agent>();
			if (agent == null)
			{
				agent = go.AddComponent<global::Kampai.Util.AI.Agent>();
			}
			agent.Radius = 0.5f;
			agent.Mass = 1f;
			agent.MaxForce = 0f;
			agent.MaxSpeed = 0f;
			return go.AddComponent<global::Kampai.Game.View.SpecialEventCharacterView>();
		}
	}
}
