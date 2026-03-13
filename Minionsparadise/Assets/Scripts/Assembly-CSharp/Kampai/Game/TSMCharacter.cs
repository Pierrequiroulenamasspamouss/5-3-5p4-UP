namespace Kampai.Game
{
	public class TSMCharacter : global::Kampai.Game.NamedCharacter<global::Kampai.Game.TSMCharacterDefinition>
	{
		public bool HasShownIntroNarrative { get; set; }

		public int PreviousTaskUTCTime { get; set; }

		public TSMCharacter(global::Kampai.Game.TSMCharacterDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					PreviousTaskUTCTime = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "HASSHOWNINTRONARRATIVE":
				reader.Read();
				HasShownIntroNarrative = global::System.Convert.ToBoolean(reader.Value);
				break;
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
			return go.AddComponent<global::Kampai.Game.View.TSMCharacterView>();
		}
	}
}
